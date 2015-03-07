using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.TraveLayer;
using TrippersStop.Helper;
using TrippersStop.Areas.Sabre.Models;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class LowFareForecastController : ApiController
    {
        // GET api/lowfareforecast
        public HttpResponseMessage Get([FromUri]LowFareForecastRequest lowFareForecastRequest)
        {
            string url = string.Format("v1/forecast/flights/fares?origin={0}&destination={1}&departuredate={2}&returndate={3}", lowFareForecastRequest.Origin, lowFareForecastRequest.Destination, lowFareForecastRequest.DepartureDate, lowFareForecastRequest.ReturnDate);
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
