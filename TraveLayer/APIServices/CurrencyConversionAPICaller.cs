using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Configuration;
using ServiceStack.Text;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.CurrencyConversion.Request;

namespace TrippismApi.TraveLayer
{
    public class CurrencyConversionAPICaller : IAsyncCurrencyConversionAPICaller 
    {
        Uri _BaseAPIUri ;
        public Uri BaseAPIUri
        {
            get{
                return this._BaseAPIUri;
            }
            set{
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
        public CurrencyConversionAPICaller()
        {
            _BaseAPIUri = new Uri(ConfigurationManager.AppSettings["OpenExchangeAPIUri"].ToString());
            _ClientId = ConfigurationManager.AppSettings["OpenExchangeClientID"].ToString();
        }
        public async Task<APIResponse> Get(string Method)
        {
            using(var client = new HttpClient())
            {
                HttpResponseMessage openExchangeResponse = await client.GetAsync(this._BaseAPIUri + "?app_id="+this._ClientId).ConfigureAwait(false);

                if (!openExchangeResponse.IsSuccessStatusCode)
                {
                    JsonObject error = await openExchangeResponse.Content.ReadAsAsync<JsonObject>();
                    string errorType = error.Get<string>("error");

                    return new APIResponse { StatusCode = openExchangeResponse.StatusCode, Response = "404", RequestUrl = this._BaseAPIUri.ToString(), OriginalResponse = openExchangeResponse };
                }
                var respose = await openExchangeResponse.Content.ReadAsStringAsync();

                return new APIResponse { StatusCode = openExchangeResponse.StatusCode, Response = respose };
            }
        }
        public async Task<APIResponse> Post(string Method, string Body)
        {
            return null;
        }
    }
}
