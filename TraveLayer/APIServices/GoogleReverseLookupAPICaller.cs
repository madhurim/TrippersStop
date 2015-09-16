using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippismApi.TraveLayer
{
    public class GoogleReverseLookupAPICaller : IAsyncGoogleReverseLookupAPICaller
    {
        Uri _BaseAPIUri;
        public Uri BaseAPIUri
        {
            get
            {
                return this._BaseAPIUri;
            }
            set
            {
                this._BaseAPIUri = value;
            }
        }
        String _ClientId;
        public String ClientId
        {
            set
            {
                this._ClientId = value;
            }
        }
        String _Accept;
        public String Accept
        {
            set
            {
                this._Accept = value;
            }
        }
        String _ContentType;
        public String ContentType
        {
            set
            {
                this._ContentType = value;
            }
        }

        public GoogleReverseLookupAPICaller()
        {
            _ClientId = ConfigurationManager.AppSettings["GooglePlaceApiKey"].ToString();
            _BaseAPIUri = new Uri(ConfigurationManager.AppSettings["GoogleReverseLookupApiUri"].ToString() + "?sensor=false&_=" + _ClientId);
        }

        public async Task<APIResponse> Get(string strLatLangQuery)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_Accept));
                HttpResponseMessage googleplaceResponse = await client.GetAsync(this.BaseAPIUri + strLatLangQuery).ConfigureAwait(false);
                var response = await googleplaceResponse.Content.ReadAsStringAsync();
                return new APIResponse { StatusCode = HttpStatusCode.OK, Response = response };
            }
        }

        public Task<APIResponse> Post(string Method, string Body)
        {
            return null;
        }
    }
}
