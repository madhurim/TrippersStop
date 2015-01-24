using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Web;
using TrippersStop.Services;

namespace TrippersStop.SabreAPIWrapper
{
    public class SabreAPICaller : IAPIAsyncCaller
    {
        Uri _TokenUri;
        
        public Uri TokenUri
        {            
            set
            {
                this._TokenUri = value; 
            }
        }
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
        String _ClientSecret;
        public String ClientSecret
        {
            set
            {
                this._ClientSecret = value;
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
        String _Authorization;
        public String Authorization
        {
            set
            {
                this._Authorization = value;
            }
        }
        public SabreAPICaller()
        {
            TokenUri = new Uri(ConfigurationManager.AppSettings["SabreTokenUri"].ToString());
            BaseAPIUri = new Uri(ConfigurationManager.AppSettings["SabreBaseAPIUri"].ToString() );
            ClientId = ConfigurationManager.AppSettings["SabreClientID"].ToString();
            ClientSecret = ConfigurationManager.AppSettings["SabreClientSecret"].ToString();
        }


        public async Task<String> GetToken()
        {
            using (var client = new HttpClient())
            {                
                string _Accept = "application/json";
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_Accept));

                string cre = String.Format("{0}:{1}", _ClientId, _ClientSecret);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(cre);
                string base64 = Convert.ToBase64String(bytes);
                client.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", base64));
                HttpContent requestContent = new StringContent("grant_type=client_credentials");
                string _ContentType = "application/x-www-form-urlencoded";
                requestContent.Headers.Add("Content-Type", _ContentType);
               
                HttpResponseMessage sabreResponse = await client.PostAsync(_TokenUri + "v1/auth/token/", requestContent);

                // If client authentication failed then we get a JSON response from Azure Market Place
                if (!sabreResponse.IsSuccessStatusCode)
                {
                    JToken error = await sabreResponse.Content.ReadAsAsync<JToken>();
                    string errorType = error.Value<string>("error");
                    string errorDescription = error.Value<string>("error_description");
                    throw new HttpRequestException(string.Format("Sabre request failed: {0} {1}", errorType, errorDescription));
                }

                // Get the access token to attach to the original request from the response body
                JToken response = await sabreResponse.Content.ReadAsAsync<JToken>();
                return HttpUtility.UrlEncode(response.Value<string>("access_token"));
            }
        }
        
        public async Task<String> Post(string Method, string Body)
        {
            using (var client = new HttpClient())
            {                               
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(this._Accept));
                HttpContent requestContent = new StringContent(Body);               
                requestContent.Headers.Add("Content-Type", _ContentType);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_Authorization, GetToken().ToString());

                //send to data market
                HttpResponseMessage sabreResponse = await client.PostAsJsonAsync(this.BaseAPIUri + Method, requestContent);

                // If client authentication failed then we get a JSON response from Sabre
                if (!sabreResponse.IsSuccessStatusCode)
                {
                    JToken error = await sabreResponse.Content.ReadAsAsync<JToken>();
                    string errorType = error.Value<string>("error");
                    string errorDescription = error.Value<string>("error_description");
                    throw new HttpRequestException(string.Format("Sabre request failed: {0} {1}", errorType, errorDescription));
                }

                return sabreResponse.ToString();
            }            
        }

        public async Task<String> Get(string Method)
        {
            using (var client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_Authorization, GetToken().ToString());

                //send to data market
                HttpResponseMessage sabreResponse = await client.GetAsync(this.BaseAPIUri + Method);

                // If client authentication failed then we get a JSON response from Sabre
                if (!sabreResponse.IsSuccessStatusCode)
                {
                    JToken error = await sabreResponse.Content.ReadAsAsync<JToken>();
                    string errorType = error.Value<string>("error");
                    string errorDescription = error.Value<string>("error_description");
                    throw new HttpRequestException(string.Format("Sabre request failed: {0} {1}", errorType, errorDescription));
                }

                return sabreResponse.ToString();
            }
        }
    }
        
}