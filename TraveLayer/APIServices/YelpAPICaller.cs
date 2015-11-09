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
using System.Security.Cryptography;
using System.Web;
using OAuth;
using SimpleOAuth;
using Newtonsoft.Json.Linq;

namespace TrippismApi.TraveLayer
{
    public class YelpAPICaller : IAsyncYelpAPICaller
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

        public YelpAPICaller()
        {
            _BaseAPIUri = new Uri(ConfigurationManager.AppSettings["YelpAPIUri"].ToString());
        }

        public async Task<APIResponse> Get(string strSearchLatitudeandsLongitude)
        {
            var client = new OAuthClient();
            var queryParams = new Dictionary<string, string>()
            {
                { "term", ConfigurationManager.AppSettings["YelpAPITerm"].ToString() },
                { "ll", strSearchLatitudeandsLongitude },
                {"limit","5"},
                {"offset","2"}



            };

            var response = await client.PerformRequestAsync(_BaseAPIUri.ToString(), queryParams);
            HttpWebResponse httpResponse = (HttpWebResponse)response;
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                JsonObject error = JsonObject.Parse(httpResponse.ToString());
                string errorType = error.Get<string>("error");
                string errorDescription = error.Get<string>("error_description");
                string message = error.Get<string>("message");
                string status = error.Get<string>("status");
                string errorMessage = error.Get<string>("message");
                string responseMessage = string.Join(" ", errorType, errorDescription, errorMessage).Trim();
                return new APIResponse { StatusCode = httpResponse.StatusCode, Response = responseMessage };
            }
            var stream = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            return new APIResponse { StatusCode = HttpStatusCode.OK, Response = JObject.Parse(stream.ReadToEnd()).ToString() };
        }

        public Task<APIResponse> Post(string Method, string Body)
        {
            return null;
        }

    }

    public class OAuthClient
    {
        public async Task<WebResponse> PerformRequestAsync(string baseURL, Dictionary<string, string> queryParams = null)
        {
            var query = System.Web.HttpUtility.ParseQueryString(String.Empty);

            if (queryParams == null)
            {
                queryParams = new Dictionary<string, string>();
            }

            foreach (var queryParam in queryParams)
            {
                query[queryParam.Key] = queryParam.Value;
            }

            var uriBuilder = new UriBuilder(baseURL);
            uriBuilder.Query = query.ToString();

            var request = WebRequest.Create(uriBuilder.ToString());
            request.Method = "GET";

            request.SignRequest(
                new Tokens
                {
                    ConsumerKey = ConfigurationManager.AppSettings["YelpConsumerKey"].ToString(),
                    ConsumerSecret = ConfigurationManager.AppSettings["YelpConsumerSecret"].ToString(),
                    AccessToken = ConfigurationManager.AppSettings["YelpToken"].ToString(),
                    AccessTokenSecret = ConfigurationManager.AppSettings["YelpTokenSecret"].ToString()
                }
            ).WithEncryption(EncryptionMethod.HMACSHA1).InHeader();

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)ex.Response;
                return httpResponse;
            }
            return response;
        }
    }
}
