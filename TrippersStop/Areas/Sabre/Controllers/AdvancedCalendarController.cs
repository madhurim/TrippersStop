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
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class AdvancedCalendarController : ApiController
    {
        IAsyncSabreAPICaller apiCaller;
        public AdvancedCalendarController(IAsyncSabreAPICaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public HttpResponseMessage Post(OTA_AdvancedCalendar advancedCalendar)
        {
            //TBD : URL configurable using XML
            String result = apiCaller.Post("v1.8.1/shop/calendar/flights?mode=live", ServiceStackSerializer.Serialize(advancedCalendar)).Result;
            var advancedCalendarResponse = DeSerializeResponse(result);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, advancedCalendarResponse);
            return response;
        }
        private LowFareSearch DeSerializeResponse(string result)
        {
            BargainFinderReponse reponse = new BargainFinderReponse();
            reponse = ServiceStackSerializer.DeSerialize<BargainFinderReponse>(result);
            Mapper.CreateMap<BargainFinderReponse, LowFareSearch>()
                    .ForMember(o => o.AirLowFareSearchRS, m => m.MapFrom(s => s.OTA_AirLowFareSearchRS));
            LowFareSearch lowFareSearch = Mapper.Map<BargainFinderReponse, LowFareSearch>(reponse);
            return lowFareSearch;
        }
    }

}
