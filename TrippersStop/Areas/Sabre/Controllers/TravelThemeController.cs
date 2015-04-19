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
    public class TravelThemeController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public TravelThemeController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/supported/shop/themes");
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
                OTA_TravelThemeLookup travelThemeLookup = new OTA_TravelThemeLookup();
                travelThemeLookup = ServiceStackSerializer.DeSerialize<OTA_TravelThemeLookup>(result.Response);
                Mapper.CreateMap<OTA_TravelThemeLookup, TravelTheme>();
                TravelTheme travelTheme = Mapper.Map<OTA_TravelThemeLookup, TravelTheme>(travelThemeLookup);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, travelTheme);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
