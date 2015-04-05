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
    
    public class CountriesController : ApiController
    {
       IAPIAsyncCaller apiCaller;
       public CountriesController(IAPIAsyncCaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        // GET api/countries
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/countries";
            return GetResponse(url);
        }
        public HttpResponseMessage Get(string pointofsalecountry)
        {
            string url = string.Format("v1/lists/supported/countries?pointofsalecountry={0}", pointofsalecountry);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            OTA_CountriesLookup cities = new OTA_CountriesLookup();
            cities = ServiceStackSerializer.DeSerialize<OTA_CountriesLookup>(result);
            Mapper.CreateMap<OTA_CountriesLookup, Countries>();
            Countries countries = Mapper.Map<OTA_CountriesLookup, Countries>(cities);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, countries);
            return response;
        }
    }
}
