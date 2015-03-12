using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.ViewModel;
using TrippersStop.Helper;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class AirportsAtCitiesController : ApiController
    {
        public HttpResponseMessage Get(string city)
        {
            string url = string.Format("v1/lists/supported/cities/{0}/airports/", city);           
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_AirportsAtCitiesLookup airportsAtCities = new OTA_AirportsAtCitiesLookup();
            airportsAtCities = ServiceStackSerializer.DeSerialize<OTA_AirportsAtCitiesLookup>(result);
            AirportsAtCities airports = Mapper.Map<OTA_AirportsAtCitiesLookup, AirportsAtCities>(airportsAtCities);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, airports);
            return response;
        }
    }
}
