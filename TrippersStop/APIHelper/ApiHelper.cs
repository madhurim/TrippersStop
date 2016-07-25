using System.Configuration;
using TraveLayer.CustomTypes.Weather;
using TrippismApi.TraveLayer;
using System.Linq;
using TrippismApi.TraveLayer.Hotel.Sabre;

namespace TrippismApi
{
    public static class ApiHelper
    {
        public static void SetApiToken(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            apiCaller.LongTermToken = cacheService.GetByKey<string>(apiCaller.SabreTokenKey);
            apiCaller.TokenExpireIn = cacheService.GetByKey<string>(apiCaller.SabreTokenExpireKey);
            if (string.IsNullOrWhiteSpace(apiCaller.LongTermToken))
            {
                string sabreAuthenticationUrl = ConfigurationManager.AppSettings["SabreAuthenticationUrl"];
                apiCaller.LongTermToken = apiCaller.GetToken(sabreAuthenticationUrl).Result;
                SaveTokenInCache(apiCaller, cacheService);
            }
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public static void SetSabreSoapApiToken(ISabreHotelSoapCaller apiCaller, ICacheService cacheService)
        {
            apiCaller.SecurityToken = cacheService.GetByKey<string>(apiCaller.SabreSessionTokenKey);
            if (string.IsNullOrWhiteSpace(apiCaller.SecurityToken))
            {
                SabreSessionCaller sabreSessionCaller = new SabreSessionCaller();
                apiCaller.SecurityToken = sabreSessionCaller.GetToken();
                string expireTime = ConfigurationManager.AppSettings.Get("SabreSoapSessionExpireInMin");
                if (!string.IsNullOrWhiteSpace(expireTime))
                {
                    cacheService.Save<string>(apiCaller.SabreSessionTokenKey, apiCaller.SecurityToken, double.Parse(expireTime));
                }

            }
        }
        private static void SaveTokenInCache(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            double expireTimeInSec;
            if (!string.IsNullOrWhiteSpace(apiCaller.TokenExpireIn) && double.TryParse(apiCaller.TokenExpireIn, out expireTimeInSec))
            {
                cacheService.Save<string>(apiCaller.SabreTokenKey, apiCaller.LongTermToken, expireTimeInSec / 60);
                cacheService.Save<string>(apiCaller.SabreTokenExpireKey, apiCaller.TokenExpireIn, expireTimeInSec / 60);
            }
        }

        public static void RefreshApiToken(ICacheService _cacheService, IAsyncSabreAPICaller _apiCaller)
        {
            _cacheService.Expire(_apiCaller.SabreTokenKey);
            _cacheService.Expire(_apiCaller.SabreTokenExpireKey);
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
        }

        public static bool IsRedisAvailable()
        {
            bool isRedisAvailable = false;
            RedisService redisService = new RedisService();
            isRedisAvailable = redisService.IsConnected();
            return isRedisAvailable;
        }
    }
}
