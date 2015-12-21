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
    public class TripAdvisorAPICaller : ITripAdvisorAPIAsyncCaller
    {
        private const string APIKeyHeader = "X-TripAdvisor-API-Key";
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
            //TBD : Pass client id in header 
            //BaseAPIUri = new Uri(ConfigurationManager.AppSettings["TripAdvisorBaseAPIUri"].ToString() + "?key=" + ClientId);
            BaseAPIUri = new Uri(ConfigurationManager.AppSettings["TripAdvisorBaseAPIUri"].ToString());
        }
        public async Task<APIResponse> Get(string Method)
        {
            //const string statusComplete = "Complete";
            //const string statusMessage = "No results were found";
            using (var client = new HttpClient())
            {   
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Accept));
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_Authorization, LongTermToken);
                client.DefaultRequestHeaders.Add(APIKeyHeader,ClientId);
                HttpResponseMessage tripAdvisorResponse = await client.GetAsync(this.BaseAPIUri + Method).ConfigureAwait(false);
                

                // If client authentication failed then we get a JSON response from Sabre
                if (!tripAdvisorResponse.IsSuccessStatusCode)
                {                   
                    JsonObject error = await tripAdvisorResponse.Content.ReadAsAsync<JsonObject>();
                    string errorType = error.Get<string>("error");
                    string message = error.Get<string>("message");
                    string code = error.Get<string>("code");
                    string type = error.Get<string>("type");
                    //string responseMessage = string.Join(" ", errorType, message, code, type).Trim();
                    //if (status == statusComplete && message == statusMessage)
                    //    return new APIResponse { StatusCode = HttpStatusCode.OK, Response =string.Empty};
                    return new APIResponse { StatusCode = tripAdvisorResponse.StatusCode, Response = error.ToString() };
                }
               
                var response = await tripAdvisorResponse.Content.ReadAsStringAsync();

                //{"DestinationLocation":"JFK","Seasonality":[{"WeekEndDate":"2016-01-10T00:00:00","YearWeekNumber":1,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2016-01-04T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2016-01-17T00:00:00","YearWeekNumber":2,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2016-01-11T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-01-18T00:00:00","YearWeekNumber":3,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-01-12T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-01-25T00:00:00","YearWeekNumber":4,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-01-19T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-02-01T00:00:00","YearWeekNumber":5,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-01-26T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-02-08T00:00:00","YearWeekNumber":6,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-02-02T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-02-15T00:00:00","YearWeekNumber":7,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-02-09T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-02-22T00:00:00","YearWeekNumber":8,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-02-16T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-03-01T00:00:00","YearWeekNumber":9,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-02-23T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-03-08T00:00:00","YearWeekNumber":10,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-03-02T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-03-15T00:00:00","YearWeekNumber":11,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-03-09T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-03-22T00:00:00","YearWeekNumber":12,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-03-16T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-03-29T00:00:00","YearWeekNumber":13,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-03-23T00:00:00","SeasonalityIndicator":"Low"},{"WeekEndDate":"2015-04-05T00:00:00","YearWeekNumber":14,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-03-30T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-04-12T00:00:00","YearWeekNumber":15,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-04-06T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-04-19T00:00:00","YearWeekNumber":16,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-04-13T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-04-26T00:00:00","YearWeekNumber":17,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-04-20T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-05-03T00:00:00","YearWeekNumber":18,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-04-27T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-05-10T00:00:00","YearWeekNumber":19,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-05-04T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-05-17T00:00:00","YearWeekNumber":20,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-05-11T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-05-24T00:00:00","YearWeekNumber":21,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-05-18T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-05-31T00:00:00","YearWeekNumber":22,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-05-25T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-06-07T00:00:00","YearWeekNumber":23,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-06-01T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-06-14T00:00:00","YearWeekNumber":24,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-06-08T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-06-21T00:00:00","YearWeekNumber":25,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-06-15T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-06-28T00:00:00","YearWeekNumber":26,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-06-22T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-07-05T00:00:00","YearWeekNumber":27,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-06-29T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-07-12T00:00:00","YearWeekNumber":28,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-07-06T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-07-19T00:00:00","YearWeekNumber":29,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-07-13T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-07-26T00:00:00","YearWeekNumber":30,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-07-20T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-08-02T00:00:00","YearWeekNumber":31,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-07-27T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-08-09T00:00:00","YearWeekNumber":32,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-08-03T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-08-16T00:00:00","YearWeekNumber":33,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-08-10T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-08-23T00:00:00","YearWeekNumber":34,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-08-17T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-08-30T00:00:00","YearWeekNumber":35,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-08-24T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-09-06T00:00:00","YearWeekNumber":36,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-08-31T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-09-13T00:00:00","YearWeekNumber":37,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-09-07T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-09-20T00:00:00","YearWeekNumber":38,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-09-14T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-09-27T00:00:00","YearWeekNumber":39,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-09-21T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-10-04T00:00:00","YearWeekNumber":40,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-09-28T00:00:00","SeasonalityIndicator":"Medium"},{"WeekEndDate":"2015-10-11T00:00:00","YearWeekNumber":41,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015-10-05T00:00:00","SeasonalityIndicator":"High"},{"WeekEndDate":"2015-10-18T00:00:00","YearWeekNumber":42,"NumberOfObservations":"GreaterThan10000","WeekStartDate":"2015
                return new APIResponse { StatusCode = HttpStatusCode.OK, Response = response }; 
            }
        }

        public Task<APIResponse> Post(string Method, string Body)
        {
            throw new NotImplementedException();
        }
    }
}
