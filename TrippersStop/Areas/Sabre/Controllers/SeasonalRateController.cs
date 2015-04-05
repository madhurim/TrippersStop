using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.TraveLayer;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class SeasonalRateController : ApiController
    {
        IAPIAsyncCaller apiCaller;
        public SeasonalRateController(IAPIAsyncCaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public HttpResponseMessage Get(string destination)
        {
            string url = string.Format("v1/historical/flights/{0}/seasonality", destination);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            OTA_TravelSeasonality seasonality = new OTA_TravelSeasonality();
            seasonality = ServiceStackSerializer.DeSerialize<OTA_TravelSeasonality>(result);
            Mapper.CreateMap<OTA_TravelSeasonality, VM.TravelSeasonality>();
            VM.TravelSeasonality travelSeasonality = Mapper.Map<OTA_TravelSeasonality, VM.TravelSeasonality>(seasonality);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, travelSeasonality);
            return response;
        }
    }
}
