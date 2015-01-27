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
            _TokenUri = new Uri(ConfigurationManager.AppSettings["SabreTokenUri"].ToString());
            _BaseAPIUri = new Uri(ConfigurationManager.AppSettings["SabreBaseAPIUri"].ToString() );
            _ClientId = ConfigurationManager.AppSettings["SabreClientID"].ToString();
            _ClientSecret = ConfigurationManager.AppSettings["SabreClientSecret"].ToString();
        }
        public async Task<String> GetToken()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_Accept));

                byte[] cidbytes = System.Text.Encoding.UTF8.GetBytes(_ClientId);
                string cidbase64 = Convert.ToBase64String(cidbytes);

                byte[] secdbytes = System.Text.Encoding.UTF8.GetBytes(_ClientSecret);
                string secdbase64 = Convert.ToBase64String(secdbytes);

                string cre = String.Format("{0}:{1}", cidbase64, secdbase64);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(cre);
                string base64 = Convert.ToBase64String(bytes);

                client.DefaultRequestHeaders.Add("Authorization", String.Format("Basic {0}", base64));

                /*POST https://api.test.sabre.com/v1/auth/token HTTP/1.1
                POST https: //api.sabre.com/v1/auth/token/ HTTP/1.1
                Accept: application/json
                Authorization: Basic VmpFNmJYRTBOMjVwTjJVNU5XeDFhM1l4ZHpwRVJWWkRSVTVVUlZJNlJWaFU6VTNkV1l6Qkhkak09
                Content-Type: application/x-www-form-urlencoded
                Host: api.test.sabre.com
                Content-Length: 29
                Expect: 100-continue
                Connection: Keep-Alive*/

                //grant_type=client_credentials
                
                HttpContent requestContent = new StringContent("grant_type=client_credentials", System.Text.Encoding.UTF8, _ContentType);
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

                //should we URLencode this ?

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