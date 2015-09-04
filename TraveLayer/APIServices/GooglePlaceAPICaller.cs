using ServiceStack.Text;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippismApi.TraveLayer
{
    public class GooglePlaceAPICaller : IAsyncGooglePlaceAPICaller
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

        public GooglePlaceAPICaller()
        {
            _ClientId = ConfigurationManager.AppSettings["GooglePlaceApiKey"].ToString();
            _BaseAPIUri = new Uri(ConfigurationManager.AppSettings["GooglePlaceDetailApiUri"].ToString() + "?key=" + _ClientId);
        }

        public async Task<APIResponse> Get(string strPlaceId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_Accept));

                HttpResponseMessage googleplaceResponse = await client.GetAsync(this.BaseAPIUri + "&placeid=" + strPlaceId).ConfigureAwait(false);

                if (!googleplaceResponse.IsSuccessStatusCode)
                {
                    var ErrorObject = googleplaceResponse.Content.ReadAsStringAsync().Result;
                    JsonObject error = JsonObject.Parse(ErrorObject);
                    string errorType = error.Get<string>("error");
                    string errorDescription = error.Get<string>("error_description");
                    string message = error.Get<string>("message");
                    string status = error.Get<string>("status");
                    string errorMessage = error.Get<string>("message");
                    string responseMessage = string.Join(" ", errorType, errorDescription, errorMessage).Trim();
                    return new APIResponse { StatusCode = googleplaceResponse.StatusCode, Response = responseMessage };
                }
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
