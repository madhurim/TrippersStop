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

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class TravelThemeController : ApiController
    {
        IAsyncSabreAPICaller apiCaller;
        public TravelThemeController(IAsyncSabreAPICaller repository, ICacheService cacheService)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            apiCaller.LongTermToken = cacheService.GetByKey<string>(apiCaller.SabreTokenKey);
            apiCaller.TokenExpireIn = cacheService.GetByKey<string>(apiCaller.SabreTokenExpireKey);
            if (string.IsNullOrWhiteSpace(apiCaller.LongTermToken))
            {
                apiCaller.LongTermToken = apiCaller.GetToken().Result;
            }
            double expireTimeInSec;
            if (!string.IsNullOrWhiteSpace(apiCaller.TokenExpireIn) && double.TryParse(apiCaller.TokenExpireIn, out expireTimeInSec))
            {
                cacheService.Save<string>(apiCaller.SabreTokenKey, apiCaller.LongTermToken, expireTimeInSec / 60);
                cacheService.Save<string>(apiCaller.SabreTokenExpireKey, apiCaller.TokenExpireIn, expireTimeInSec / 60);
            }

            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public HttpResponseMessage Get()
        {
            string url = string.Format("v1/lists/supported/shop/themes");
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            OTA_TravelThemeLookup travelThemeLookup = new OTA_TravelThemeLookup();
            travelThemeLookup = ServiceStackSerializer.DeSerialize<OTA_TravelThemeLookup>(result);
            Mapper.CreateMap<OTA_TravelThemeLookup, TravelTheme>();
            TravelTheme travelTheme = Mapper.Map<OTA_TravelThemeLookup, TravelTheme>(travelThemeLookup);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, travelTheme);
            return response;
        }
    }
}
