using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.Helper;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class CityPairsController : ApiController
    {
        public HttpResponseMessage Get(string type)
        {
            string url=string.Empty;
            switch (type)
            {
                case "AirSearch":
                    url="v1/lists/airports/supported/origins-destinations";
                    break;
                case "FareRange":
                    url="v1/lists/supported/historical/flights/origins-destinations";
                    break;
                case "LowFareForecast":
                    url="v1/lists/supported/forecast/flights/origins-destinations";
                    break;
                default:
                   return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
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
