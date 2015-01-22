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
            get
            {
                return this._TokenUri;
            }
            set
            {
                this._TokenUri = new Uri("https://api.test.sabre.com/"); 
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
                this._BaseAPIUri = new Uri("https://api.test.sabre.com/");
            }
        }
        String _ClientId;
        public String ClientId
        {
            set
            {
                this._ClientId = ConfigurationManager.AppSettings["ClientID"].ToString();
            }
        }
        String _ClientSecret;
        public String ClientSecret
        {
            set
            {
                this._ClientSecret = ConfigurationManager.AppSettings["ClientID"].ToString();
            }
        }
       
        public async Task<String> GetToken()
        {
            using (var client = new HttpClient())
            {                
                // Create form parameters that we will send to data market.
                Dictionary<string, string> requestDetails = new Dictionary<string, string>
                {
                    //auth_flow:client_cred
                    { "grant_type", "client_credentials" },
                    { "client_id",   _ClientId},
                    { "client_secret",  _ClientSecret },
                    
                };

                FormUrlEncodedContent requestContent = new FormUrlEncodedContent(requestDetails);                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                //send to data market
                HttpResponseMessage sabreResponse = await client.PostAsync(this.TokenUri + "io-docs/getoauth2accesstoken", requestContent);

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
        public async Task<String> PostUrlEncoded(string token, string accessUri, Dictionary<string, string> requestDetails)
        {
            using (var client = new HttpClient())
            {
               /* Dictionary<string, string> requestDetails = new Dictionary<string, string>
                {
                    { "origin" , "CLT"},
                    { "departuredate" , "2014-03-15"},
                    { "returndate" , "2014-03-25"},
                    { "theme" , "2014-03-25"},
                };*/

                FormUrlEncodedContent requestContent = new FormUrlEncodedContent(requestDetails);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);                

                //send to data market
                HttpResponseMessage sabreResponse = await client.PostAsync(this.BaseAPIUri + accessUri, requestContent);

                // If client authentication failed then we get a JSON response from Sabre
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
        public async Task<String> Post<T>(string token, string accessUri, T value)
        {
            using (var client = new HttpClient())
            {
                               
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //send to data market
                HttpResponseMessage sabreResponse = await client.PostAsJsonAsync(this.BaseAPIUri + accessUri, value);

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

        public async Task<String> Get(string token, string accessUri)
        {
            using (var client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                //send to data market
                HttpResponseMessage sabreResponse = await client.GetAsync(this.BaseAPIUri + accessUri);

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