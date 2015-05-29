using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// API retrieves a list of destination airport and multi-airport city (MAC) codes that are associated with the theme in the request
    /// </summary>
    public class ThemeAirportController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public ThemeAirportController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;         
        }
        /// <summary>
        /// To get a reference of destination airport codes that have been associated with a theme
        /// </summary>
        public HttpResponseMessage Get(string theme)
        {
            string url = string.Format("v1/shop/themes/{0}", theme);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SabreApiTokenHelper.RefreshApiToken(_cacheService, _apiCaller);
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
