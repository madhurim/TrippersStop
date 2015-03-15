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
    public class CountriesController : ApiController
    {
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
            string result = APIHelper.GetDataFromSabre(url);
            OTA_CountriesLookup cities = new OTA_CountriesLookup();
            cities = ServiceStackSerializer.DeSerialize<OTA_CountriesLookup>(result);
            Mapper.CreateMap<OTA_CountriesLookup, Countries>();
            Countries countries = Mapper.Map<OTA_CountriesLookup, Countries>(cities);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, countries);
            return response;
        }
    }
}
