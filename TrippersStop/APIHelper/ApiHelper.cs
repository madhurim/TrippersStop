using System.Configuration;
using TraveLayer.CustomTypes.Weather;
using TrippismApi.TraveLayer;
using System.Linq;
using TAVM=TraveLayer.CustomTypes.TripAdvisor.ViewModels;
using RS=TraveLayer.CustomTypes.TripAdvisor.Response;
using Trippism.APIHelper;
using TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels;
using TraveLayer.SoapServices.Hotel.Sabre.HotelAvailabilityRequest;
using TraveLayer.SoapServices.Hotel.Sabre;
using TraveLayer.SoapServices.Hotel;

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
        public static void SetSabreSoapApiToken(ISabreHotel apiCaller, ICacheService cacheService)
        {
            apiCaller.SecurityToken = cacheService.GetByKey<string>(apiCaller.SabreSessionTokenKey);
            if (string.IsNullOrWhiteSpace(apiCaller.SecurityToken))
            {
                SabreSessionCaller sabreSessionCaller = new SabreSessionCaller();
                apiCaller.SecurityToken = sabreSessionCaller.GetToken();
                string expireTime = ConfigurationManager.AppSettings.Get("SabreSoapSessionExpireInMin");
                if(!string.IsNullOrWhiteSpace(expireTime))
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

        public static void FilterChanceRecord(Trip trip, TripWeather tripWeather)
        {
            if (trip.chance_of.chanceofcloudyday != null && IsValidRecord(trip.chance_of.chanceofcloudyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofcloudyday, "chanceofcloudyday");
            }
            if (trip.chance_of.chanceoffogday != null && IsValidRecord(trip.chance_of.chanceoffogday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceoffogday, "chanceoffogday");
            }
            if (trip.chance_of.chanceofhailday != null && IsValidRecord(trip.chance_of.chanceofhailday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofhailday, "chanceofhailday");
            }
            if (trip.chance_of.chanceofhumidday != null && IsValidRecord(trip.chance_of.chanceofhumidday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofhumidday, "chanceofhumidday");
            }
            if (trip.chance_of.chanceofpartlycloudyday != null && IsValidRecord(trip.chance_of.chanceofpartlycloudyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofpartlycloudyday, "chanceofpartlycloudyday");
            }
            if (trip.chance_of.chanceofprecip != null && IsValidRecord(trip.chance_of.chanceofprecip))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofprecip, "chanceofprecip");
            }
            if (trip.chance_of.chanceofrainday != null && IsValidRecord(trip.chance_of.chanceofrainday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofrainday, "chanceofrainday");
            }
            if (trip.chance_of.chanceofsnowday != null && IsValidRecord(trip.chance_of.chanceofsnowday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsnowday, "chanceofsnowday");
            }
            if (trip.chance_of.chanceofsnowonground != null && IsValidRecord(trip.chance_of.chanceofsnowonground))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsnowonground, "chanceofsnowonground");
            }
            if (trip.chance_of.chanceofsultryday != null && IsValidRecord(trip.chance_of.chanceofsultryday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsultryday, "chanceofsultryday");
            }
            if (trip.chance_of.chanceofsunnycloudyday != null && IsValidRecord(trip.chance_of.chanceofsunnycloudyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofsunnycloudyday, "chanceofsunnycloudyday");
            }
            if (trip.chance_of.chanceofthunderday != null && IsValidRecord(trip.chance_of.chanceofthunderday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofthunderday, "chanceofthunderday");
            }
            if (trip.chance_of.chanceoftornadoday != null && IsValidRecord(trip.chance_of.chanceoftornadoday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceoftornadoday, "chanceoftornadoday");
            }
            if (trip.chance_of.chanceofwindyday != null && IsValidRecord(trip.chance_of.chanceofwindyday))
            {
                AddChanceOfRecord(tripWeather, trip.chance_of.chanceofwindyday, "chanceofwindyday");
            }
        }

        public static void AddChanceOfRecord(TripWeather tripWeather, Chance propertyValue, string propertyName)
        {
            tripWeather.WeatherChances.Add(new WeatherChance()
            {
                ChanceType = propertyName,
                Description = propertyValue.description,
                Name = propertyValue.name,
                Percentage = propertyValue.percentage
            });
        }

        public static bool IsValidRecord(Chance chance)
        {
            if (chance != null && chance.percentage > 30)
            {
                return true;
            }
            return false;
        }


        public static bool IsRedisAvailable()
        {
            bool isRedisAvailable = false;
            RedisService redisService = new RedisService();
            isRedisAvailable = redisService.IsConnected();
            return isRedisAvailable;
        }

        public static void CopyPropertiesTo<T, TU>(this T source, TU dest)
        {

            var sourceProps = typeof(T).GetProperties().Where(x => x.CanRead).ToList();
            var destProps = typeof(TU).GetProperties()
                    .Where(x => x.CanWrite)
                    .ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }

            }

        }

   
    }
}
