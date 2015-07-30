using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.Response;
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
        /// <summary>
        /// Returns a weather summary based on historical information between the specified dates (30 days max).
        /// </summary>
        public HistoryController(IAsyncWeatherAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;           
        }
        /// <summary>
        /// Returns a weather summary based on historical information between the specified dates (30 days max).
        /// </summary>
        [ResponseType(typeof(TripWeather))]
        [Route("api/weather/history")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]HistoryInput weatherInfo)
        {
            string cacheKey = TrippismKey+string.Join(".", weatherInfo.City , weatherInfo.State , weatherInfo.DepartDate.ToShortDateString() , weatherInfo.ReturnDate.ToShortDateString());
            var tripWeather = _cacheService.GetByKey<TripWeather>(cacheKey);
            if (tripWeather != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripWeather);
            }
            //http://localhost:14606/api/weather/history?State=CA&City=San_Francisco&DepartDate=2015-07-06&ReturnDate=2015-07-09
            // http://api.wunderground.com/api/Your_Key/planner_MMDDMMDD/q/CA/San_Francisco.json
            string fromDate = weatherInfo.DepartDate.ToString("MMdd");
            string toDate = weatherInfo.ReturnDate.ToString("MMdd");
            string url = string.Format("planner_{0}{1}/q/{2}/{3}.json", fromDate, toDate, weatherInfo.State, weatherInfo.City);
            return GetResponse(url, cacheKey);
        }




        [ResponseType(typeof(TripWeather))]
        [Route("api/weather/international/history")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]InternationalWeatherInput weatherInfo)
        {
            string cacheKey = TrippismKey + string.Join(".", weatherInfo.AirportCode, weatherInfo.CountryCode, weatherInfo.DepartDate.ToShortDateString(), weatherInfo.ReturnDate.ToShortDateString());
            var tripWeather = _cacheService.GetByKey<TripWeather>(cacheKey);
            if (tripWeather != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripWeather);
            }
            //http://localhost:14606/api/weather/history?State=CA&City=San_Francisco&DepartDate=2015-07-06&ReturnDate=2015-07-09
            //http://api.wunderground.com/api/my_key/planner_10011031/q/IN/Karwar.json
            string fromDate = weatherInfo.DepartDate.ToString("MMdd");
            string toDate = weatherInfo.ReturnDate.ToString("MMdd");
            string url = string.Format("planner_{0}{1}/q/{2}/{3}.json", fromDate, toDate, weatherInfo.CountryCode, weatherInfo.AirportCode);
            return GetResponse(url, cacheKey);
        }





        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";
            APIResponse result = _apiCaller.Get(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                HistoryOutput weather = new HistoryOutput();
                weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(result.Response);
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
            return Request.CreateResponse(result.StatusCode, result.Response);
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
            }
                );
        }

        private bool IsValidRecord(Chance chance)
        {
            if (chance !=null && chance.percentage > 30)
            {
                return true;
            }
            return false;
        }
    }
}
