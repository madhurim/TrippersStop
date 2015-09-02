using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.APIExtention.Filters;
using TrippismApi.TraveLayer;
using VM = TraveLayer.CustomTypes.Sabre.ViewModels;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// AdvancedCalendar is used for -
    /// Consumers who want to shop across a large set of travel dates. 
    /// Consumers who know their destination, but are flexible on their travel dates and want to search flight options across a large set or range of travel dates, or lengths of stay.
    /// Consumers who want to shop across several specific shopping parameters.
    /// </summary>

  [GZipCompressionFilter]
    public class AdvancedCalendarController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public AdvancedCalendarController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        /// <summary>
        /// To get priced air itineraries to a destination airport on specific roundtrip travel dates.
        /// </summary>
        [ResponseType(typeof(LowFareSearch))]
        public HttpResponseMessage Post(OTA_AdvancedCalendar advancedCalendar)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            //TBD : URL configurable using XML
            string url="v1.8.1/shop/calendar/flights?mode=live";
            APIResponse result = _apiCaller.Post(url, ServiceStackSerializer.Serialize(advancedCalendar)).Result;            
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Post(url, ServiceStackSerializer.Serialize(advancedCalendar)).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var advancedCalendarResponse = DeSerializeResponse(result.Response);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, advancedCalendarResponse);
                return response;
            }
            return Request.CreateResponse(result.StatusCode,result.Response);
        }
        /// <summary>
        /// Serialize the json reponse
        /// </summary>
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
