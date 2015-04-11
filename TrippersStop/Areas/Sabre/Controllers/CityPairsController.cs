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
    public class CityPairsController : ApiController
    {
        IAsyncSabreAPICaller apiCaller;
        public CityPairsController(IAsyncSabreAPICaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
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
            String result = apiCaller.Get(url).Result;
            OTA_CityPairsLookup cities = new OTA_CityPairsLookup();
            cities = ServiceStackSerializer.DeSerialize<OTA_CityPairsLookup>(result);
            Mapper.CreateMap<OTA_AirportsAtCitiesLookup, CityPairs>();
            CityPairs cityPairs = Mapper.Map<OTA_CityPairsLookup, CityPairs>(cities);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, cityPairs);
            return response;
        }
    }
}
