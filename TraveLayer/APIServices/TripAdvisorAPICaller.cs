using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;

namespace TrippismApi.TraveLayer
{
    //TBD : Need to remove refrence of ASabre response. Need to move location of API Response
    public class TripAdvisorAPICaller : ITripAdvisorAPIAsyncCaller
    {
        public  Uri BaseAPIUri
        {
            get;
            set;
        }

        public String ClientId
        {
            get;
            set;
        }

        public String Accept
        {
            get;
            set;
        }
   
        public String ContentType
        {
            get;
            set;
        }
        public TripAdvisorAPICaller()
        {
            ClientId = ConfigurationManager.AppSettings["TripAdvisorKey"].ToString();
            BaseAPIUri = new Uri(ConfigurationManager.AppSettings["TripAdvisorBaseAPIUri"].ToString());
        }
        public async Task<APIResponse> Get(string Method)
        {
            using (var client = new HttpClient())
            {
                string apiUrl = string.Format(this.BaseAPIUri + Method, ClientId);
                HttpResponseMessage tripAdvisorResponse = await client.GetAsync(apiUrl).ConfigureAwait(false);              
                if (!tripAdvisorResponse.IsSuccessStatusCode)
                {                   
                    //JsonObject error = await tripAdvisorResponse.Content.ReadAsAsync<JsonObject>();
                    //string errorType = error.Get<string>("error");
                    //string message = error.Get<string>("message");
                    //string code = error.Get<string>("code");
                    //string type = error.Get<string>("type");
                    return new APIResponse { StatusCode = tripAdvisorResponse.StatusCode, Response = string.Empty};
                }            
                var response = await tripAdvisorResponse.Content.ReadAsStringAsync();
                return new APIResponse { StatusCode = HttpStatusCode.OK, Response = response }; 
            }
        }

        public Task<APIResponse> Post(string Method, string Body)
        {
            throw new NotImplementedException();
        }
    }
}
