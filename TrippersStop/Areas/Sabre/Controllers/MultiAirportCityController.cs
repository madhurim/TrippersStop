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
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// API retrieves a list of multi-airport city (MAC) codes.
    /// </summary>
    public class MultiAirportCityController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public MultiAirportCityController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        /// <summary>
        /// API retrieves a list of multi-airport city (MAC) codes based on country.
        /// Multi-airport cities located in the requested country(s), sorted by city name, in ascending rank order. If no country is specified, then all MAC codes and cities are returned
        /// </summary>
        [ResponseType(typeof(MultiAirportCity))]
        public HttpResponseMessage Get(string country)
        {
            string url = string.Format("v1/lists/supported/cities?country={0}", country);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_MultiAirportCityLookup airports = new OTA_MultiAirportCityLookup();
                airports = ServiceStackSerializer.DeSerialize<OTA_MultiAirportCityLookup>(result.Response);
                Mapper.CreateMap<OTA_MultiAirportCityLookup, MultiAirportCity>();
                MultiAirportCity multiAirportCity = Mapper.Map<OTA_MultiAirportCityLookup, MultiAirportCity>(airports);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, multiAirportCity);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
