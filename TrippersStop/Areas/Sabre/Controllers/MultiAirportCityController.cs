using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.TraveLayer;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class MultiAirportCityController : ApiController
    {
        IAsyncSabreAPICaller apiCaller;
        public MultiAirportCityController(IAsyncSabreAPICaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public HttpResponseMessage Get(string country)
        {
            string url = string.Format("v1/lists/supported/cities?country={0}", country);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            OTA_MultiAirportCityLookup airports = new OTA_MultiAirportCityLookup();
            airports = ServiceStackSerializer.DeSerialize<OTA_MultiAirportCityLookup>(result);
            Mapper.CreateMap<OTA_MultiAirportCityLookup, MultiAirportCity>();
            MultiAirportCity multiAirportCity = Mapper.Map<OTA_MultiAirportCityLookup, MultiAirportCity>(airports);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, multiAirportCity);
            return response;
        }
    }
}
