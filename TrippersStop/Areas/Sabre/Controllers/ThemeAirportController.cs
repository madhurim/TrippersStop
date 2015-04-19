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
    public class ThemeAirportController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public ThemeAirportController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        public HttpResponseMessage Get(string theme)
        {
            string url = string.Format("v1/shop/themes/{0}", theme);
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            APIHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                APIHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_ThemeAirportLookup themeAirportLookup = new OTA_ThemeAirportLookup();
                themeAirportLookup = ServiceStackSerializer.DeSerialize<OTA_ThemeAirportLookup>(result.Response);
                Mapper.CreateMap<OTA_ThemeAirportLookup, ThemeAirport>();
                ThemeAirport themeAirport = Mapper.Map<OTA_ThemeAirportLookup, ThemeAirport>(themeAirportLookup);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, themeAirport);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
