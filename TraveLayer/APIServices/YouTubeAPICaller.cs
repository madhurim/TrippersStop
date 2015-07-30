using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippismApi.TraveLayer
{
    public class YouTubeAPICaller : IAsyncYouTubeAPICaller
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

        public YouTubeAPICaller()
        {
            _ClientId = ConfigurationManager.AppSettings["YouTubeApiKey"].ToString();
            _BaseAPIUri = new Uri(ConfigurationManager.AppSettings["YouTubeBaseAPIUri"].ToString() + "&key=" + _ClientId);
        }

        public async Task<APIResponse> Get(string strSearchLatitudeandsLongitude)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_Accept));

                HttpResponseMessage youtubeResponse = await client.GetAsync(this.BaseAPIUri + "&location=" + strSearchLatitudeandsLongitude).ConfigureAwait(false);

                if (!youtubeResponse.IsSuccessStatusCode)
                {
                    var ErrorObject = youtubeResponse.Content.ReadAsStringAsync().Result;
                    JsonObject error = JsonObject.Parse(ErrorObject);
                    string errorType = error.Get<string>("error");
                    string errorDescription = error.Get<string>("error_description");
                    string message = error.Get<string>("message");
                    string status = error.Get<string>("status");
                    string errorMessage = error.Get<string>("message");
                    string responseMessage = string.Join(" ", errorType, errorDescription, errorMessage).Trim();
                    return new APIResponse { StatusCode = youtubeResponse.StatusCode, Response = responseMessage };
                }
                var response = await youtubeResponse.Content.ReadAsStringAsync();
                return new APIResponse { StatusCode = HttpStatusCode.OK, Response = response };
            }
        }

        public Task<APIResponse> Post(string Method, string Body)
        {
            return null;
        }
    }
}
