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
using TrippersStop.Helper;
using TrippersStop.TraveLayer;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class AdvancedCalendarController : ApiController
    {
        public HttpResponseMessage Post(OTA_AdvancedCalendar advancedCalendar)
        {
            SabreAPICaller sabreAPICaller = new SabreAPICaller();
            sabreAPICaller.Accept = "application/json";
            sabreAPICaller.ContentType = "application/x-www-form-urlencoded";
            //TBD : Aoid call for getting token
            string token = sabreAPICaller.GetToken().Result;
            sabreAPICaller.Authorization = "bearer";
            sabreAPICaller.ContentType = "application/json";
            //TBD : URL configurable using XML
            String result = sabreAPICaller.Post("v1.8.1/shop/calendar/flights?mode=live", ServiceStackSerializer.Serialize(advancedCalendar)).Result;
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
