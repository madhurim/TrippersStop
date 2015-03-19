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
    public class MultiAirportCityController : ApiController
    {
        public HttpResponseMessage Get(string country)
        {
            string url = string.Format("v1/lists/supported/cities?country={0}", country);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_MultiAirportCityLookup airports = new OTA_MultiAirportCityLookup();
            airports = ServiceStackSerializer.DeSerialize<OTA_MultiAirportCityLookup>(result);
            Mapper.CreateMap<OTA_MultiAirportCityLookup, MultiAirportCity>();
            MultiAirportCity multiAirportCity = Mapper.Map<OTA_MultiAirportCityLookup, MultiAirportCity>(airports);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, multiAirportCity);
            return response;
        }
    }
}
