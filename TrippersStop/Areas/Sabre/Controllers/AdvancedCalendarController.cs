using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.TraveLayer;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;

namespace Trippism.Areas.Sabre.Controllers
{
    public class AdvancedCalendarController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public AdvancedCalendarController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        public HttpResponseMessage Post(OTA_AdvancedCalendar advancedCalendar)
        {
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            //TBD : URL configurable using XML
            string url="v1.8.1/shop/calendar/flights?mode=live";
            APIResponse result = _apiCaller.Post(url, ServiceStackSerializer.Serialize(advancedCalendar)).Result;            
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SabreApiTokenHelper.RefreshApiToken(_cacheService, _apiCaller);             
                result = _apiCaller.Post("v1.8.1/shop/calendar/flights?mode=live", ServiceStackSerializer.Serialize(advancedCalendar)).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var advancedCalendarResponse = DeSerializeResponse(result.Response);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, advancedCalendarResponse);
                return response;
            }
            return Request.CreateResponse(result.StatusCode,result.Response);
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
