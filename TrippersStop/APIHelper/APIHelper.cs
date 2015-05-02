using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trippism.TraveLayer;

namespace Trippism
{
    public static class APIHelper
    {
        public static void SetApiToken(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
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

        public static void RefreshApiToken(ICacheService _cacheService, IAsyncSabreAPICaller _apiCaller)
        {
            _cacheService.Expire(_apiCaller.SabreTokenKey);
            _cacheService.Expire(_apiCaller.SabreTokenExpireKey);
             APIHelper.SetApiToken(_apiCaller, _cacheService);
        }
    }
}