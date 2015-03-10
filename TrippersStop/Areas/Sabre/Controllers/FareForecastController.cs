using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.TraveLayer;
using TrippersStop.Helper;
using TraveLayer.CustomTypes.ViewModel;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class FareForecastController : ApiController
    {
        // GET api/lowfareforecast
        public HttpResponseMessage Get([FromUri]FareForecast fareForecastRequest)
        {
            string url = string.Format("v1/historical/flights/fares?origin={0}&destination={1}&earliestdeparturedate={2}&latestdeparturedate={3}&lengthofstay={4}", fareForecastRequest.Origin, fareForecastRequest.Destination, fareForecastRequest.EarliestDepartureDate, fareForecastRequest.LatestDepartureDate, fareForecastRequest.LengthOfStay);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}
