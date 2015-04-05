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
    public class AirportsAtCitiesController : ApiController
    {
         IAPIAsyncCaller apiCaller;
         public AirportsAtCitiesController(IAPIAsyncCaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public HttpResponseMessage Get(string city)
        {
            string url = string.Format("v1/lists/supported/cities/{0}/airports/", city);           
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            OTA_AirportsAtCitiesLookup airportsAtCities = new OTA_AirportsAtCitiesLookup();
            airportsAtCities = ServiceStackSerializer.DeSerialize<OTA_AirportsAtCitiesLookup>(result);
            Mapper.CreateMap<OTA_AirportsAtCitiesLookup, AirportsAtCities>();
            AirportsAtCities airports = Mapper.Map<OTA_AirportsAtCitiesLookup, AirportsAtCities>(airportsAtCities);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, airports);
            return response;
        }
    }
}
