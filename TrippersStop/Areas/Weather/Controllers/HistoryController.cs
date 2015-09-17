using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Weather;
using TraveLayer.CustomTypes.Weather.Request;
using TrippismApi;
using TrippismApi.TraveLayer;
namespace Trippism.Areas.Weather.Controllers
{
    public class HistoryController : ApiController
    {
        const string TrippismKey = "Trippism.Weather.";
        IAsyncWeatherAPICaller _apiCaller;
        ICacheService _cacheService;
        IAsyncGoogleReverseLookupAPICaller _apiGoogleReverseLookupCaller;
        /// <summary>
        /// Returns a weather summary based on historical information between the specified dates (30 days max).
        /// </summary>
        public HistoryController(IAsyncGoogleReverseLookupAPICaller apiGoogleReverseLookupCaller, IAsyncWeatherAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _apiGoogleReverseLookupCaller = apiGoogleReverseLookupCaller;
        }
        /// <summary>
        /// Returns a weather summary based on historical information between the specified dates (30 days max).
        /// </summary>
        [ResponseType(typeof(TripWeather))]
        [Route("api/weather/history")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]HistoryInput weatherInfo)
        {
            //http://localhost:14606/api/weather/history?State=CA&City=San_Francisco&DepartDate=2015-07-06&ReturnDate=2015-07-09
            // http://api.wunderground.com/api/Your_Key/planner_MMDDMMDD/q/CA/San_Francisco.json

            string cacheKey = TrippismKey + string.Join(".", weatherInfo.Latitude, weatherInfo.Longitude, weatherInfo.DepartDate.ToShortDateString(), weatherInfo.ReturnDate.ToShortDateString());
            var tripWeather = _cacheService.GetByKey<TripWeather>(cacheKey);
            if (tripWeather != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripWeather);

            #region Getting StateCode
            Trippism.Areas.Google.Controllers.GoogleGeoCodeController googleGeoCode = new Google.Controllers.GoogleGeoCodeController(_apiGoogleReverseLookupCaller, _cacheService);
            TraveLayer.CustomTypes.Google.Request.GoogleInput googleInput = new TraveLayer.CustomTypes.Google.Request.GoogleInput() { Latitude = weatherInfo.Latitude, Longitude = weatherInfo.Longitude };
            TraveLayer.CustomTypes.Google.ViewModels.GoogleReverseLookup googleReverseLookup = await googleGeoCode.Get(googleInput);
            weatherInfo.State = googleReverseLookup.StateCode;
            #endregion

            string fromDate = weatherInfo.DepartDate.ToString("MMdd");
            string toDate = weatherInfo.ReturnDate.ToString("MMdd");

            Trippism.Areas.Weather.StaticMethods.WeatherAPIResponse._apiCaller = _apiCaller;
            Trippism.Areas.Weather.StaticMethods.WeatherAPIResponse._cacheService = _cacheService;
            var apiResult = await Task.Run(() => Trippism.Areas.Weather.StaticMethods.WeatherAPIResponse.GetWeatherAPIResult(fromDate, toDate, weatherInfo.State, weatherInfo.City));
            if (apiResult.StatusCode == HttpStatusCode.OK)
                return await Task.Run(() => { return GetResponseDetail(apiResult.Response, cacheKey); });
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get data for non US cities
        /// </summary>
        [ResponseType(typeof(TripWeather))]
        [Route("api/weather/international/history")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]InternationalWeatherInput weatherInfo)
        {
            //http://localhost:14606/api/weather/history?State=CA&City=San_Francisco&DepartDate=2015-07-06&ReturnDate=2015-07-09
            //http://api.wunderground.com/api/my_key/planner_10011031/q/IN/Karwar.json

            string cacheKey = TrippismKey + string.Join(".", weatherInfo.AirportCode, weatherInfo.CountryCode, weatherInfo.DepartDate.ToShortDateString(), weatherInfo.ReturnDate.ToShortDateString());
            var tripWeather = _cacheService.GetByKey<TripWeather>(cacheKey);
            if (tripWeather != null)
                return Request.CreateResponse(HttpStatusCode.OK, tripWeather);

            string fromDate = weatherInfo.DepartDate.ToString("MMdd");
            string toDate = weatherInfo.ReturnDate.ToString("MMdd");

            Trippism.Areas.Weather.StaticMethods.WeatherAPIResponse._apiCaller = _apiCaller;
            Trippism.Areas.Weather.StaticMethods.WeatherAPIResponse._cacheService = _cacheService;
            var apiResult = await Task.Run(() => Trippism.Areas.Weather.StaticMethods.WeatherAPIResponse.GetWeatherAPIResult(fromDate, toDate, weatherInfo.CountryCode, weatherInfo.CityName));
            if (apiResult.StatusCode == HttpStatusCode.OK)
                return await Task.Run(() => { return GetResponseDetail(apiResult.Response, cacheKey); });
            else
                return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private HttpResponseMessage GetResponseDetail(string Response, string cacheKey)
        {
            HistoryOutput weather = new HistoryOutput();
            weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(Response);
            if (weather != null && weather.trip != null)
            {
                Trip trip = weather.trip;
                Mapper.CreateMap<Trip, TripWeather>()
                   .ForMember(h => h.TempHighAvg, m => m.MapFrom(s => s.temp_high))
                   .ForMember(h => h.TempLowAvg, m => m.MapFrom(s => s.temp_low))
                   .ForMember(h => h.CloudCover, m => m.MapFrom(s => s.cloud_cover));
                Mapper.CreateMap<TempHigh, TempHighAvg>()
                    .ForMember(h => h.Avg, m => m.MapFrom(s => s.avg));
                Mapper.CreateMap<TempLow, TempLowAvg>()
                   .ForMember(h => h.Avg, m => m.MapFrom(s => s.avg));
                TripWeather tripWeather = Mapper.Map<Trip, TripWeather>(trip);
                tripWeather.WeatherChances = new List<WeatherChance>();
                if (trip.chance_of != null)
                {
                    ApiHelper.FilterChanceRecord(trip, tripWeather);
                }
                _cacheService.Save<TripWeather>(cacheKey, tripWeather);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, tripWeather);
                return response;
            }
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        private void FilterChanceRecord(Trip trip, TripWeather tripWeather)
        {
            Type type = trip.chance_of.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            Chance propertyValue = null;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
                var result = property.GetValue(trip.chance_of, null);
                if (result != null)
                {
                    propertyValue = result as Chance;
                    bool addToCollection = IsValidRecord(propertyValue);
                    if (addToCollection)
                        AddChanceOfRecord(tripWeather, propertyValue, propertyName);
                }
            }
        }

        private void AddChanceOfRecord(TripWeather tripWeather, Chance propertyValue, string propertyName)
        {
            tripWeather.WeatherChances.Add(new WeatherChance()
            {
                ChanceType = propertyName,
                Description = propertyValue.description,
                Name = propertyValue.name,
                Percentage = propertyValue.percentage
            });
        }

        private bool IsValidRecord(Chance chance)
        {
            if (chance != null && chance.percentage > 30)
            {
                return true;
            }
            return false;
        }
    }
}
