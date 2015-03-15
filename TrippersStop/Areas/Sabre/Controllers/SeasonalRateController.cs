using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModel;
using TrippersStop.Helper;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class SeasonalRateController : ApiController
    {
        public HttpResponseMessage Get(string destination)
        {
            string url = string.Format("v1/historical/flights/{0}/seasonality", destination);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_TravelSeasonality seasonality = new OTA_TravelSeasonality();
            seasonality = ServiceStackSerializer.DeSerialize<OTA_TravelSeasonality>(result);
            Mapper.CreateMap<OTA_TravelSeasonality, TravelSeasonality>();
            TravelSeasonality travelSeasonality = Mapper.Map<OTA_TravelSeasonality, TravelSeasonality>(seasonality);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, travelSeasonality);
            return response;
        }
    }
}
