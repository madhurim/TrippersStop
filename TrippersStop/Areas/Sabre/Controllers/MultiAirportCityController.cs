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
using TrippersStop.TraveLayer;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class MultiAirportCityController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public MultiAirportCityController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        public HttpResponseMessage Get(string country)
        {
            string url = string.Format("v1/lists/supported/cities?country={0}", country);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            APIHelper.SetApiKey(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                _cacheService.Expire(_apiCaller.SabreTokenKey);
                _cacheService.Expire(_apiCaller.SabreTokenExpireKey);
                APIHelper.SetApiKey(_apiCaller, _cacheService);
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
