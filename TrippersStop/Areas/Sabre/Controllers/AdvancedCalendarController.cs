using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.Helper;
using VM=TraveLayer.CustomTypes.Sabre.ViewModels;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class AdvancedCalendarController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string url = "v1.8.1/shop/calendar/flights";
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_AdvancedCalendar airportsAtCities = new OTA_AdvancedCalendar();
            airportsAtCities = ServiceStackSerializer.DeSerialize<OTA_AdvancedCalendar>(result);
            Mapper.CreateMap<OTA_AdvancedCalendar, VM.AdvancedCalendar>();
            VM.AdvancedCalendar airports = Mapper.Map<OTA_AdvancedCalendar, VM.AdvancedCalendar>(airportsAtCities);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, airports);
            return response;
        }
    }

}
