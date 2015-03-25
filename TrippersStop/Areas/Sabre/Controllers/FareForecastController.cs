using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrippersStop.TraveLayer;
using TrippersStop.Helper;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.APIServices;
using AutoMapper;
using TraveLayer.CustomTypes.Sabre;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class FareForecastController : ApiController
    {
        // GET api/lowfareforecast
        public HttpResponseMessage Get([FromUri]VM.FareForecast  fareForecastRequest)
        {
            string url = string.Format("v1/historical/flights/fares?origin={0}&destination={1}&earliestdeparturedate={2}&latestdeparturedate={3}&lengthofstay={4}", fareForecastRequest.Origin, fareForecastRequest.Destination, fareForecastRequest.EarliestDepartureDate, fareForecastRequest.LatestDepartureDate, fareForecastRequest.LengthOfStay);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_FareRange fares = new OTA_FareRange();
            fares = ServiceStackSerializer.DeSerialize<OTA_FareRange>(result);
            Mapper.CreateMap<OTA_FareRange, VM.FareRange>();
            VM.FareRange fareRange = Mapper.Map<OTA_FareRange, VM.FareRange>(fares);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fareRange);
            return response;
        }
    }
}
