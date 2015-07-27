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
    ///  API retrieves a list of themes
    /// </summary>
    public class TravelThemeController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public TravelThemeController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        /// <summary>
        ///  API retrieves a list of themes
        /// </summary>
        [ResponseType(typeof(TravelTheme))]
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/supported/shop/themes");
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
