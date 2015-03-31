using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Weather;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.Weather.Controllers
{
    public class HistoryController : ApiController
    {
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

            //http://localhost:14606/api/weather/history?State=CA&City=San_Francisco&DepartDate=2015-07-06&ReturnDate=2015-07-09
            // http://api.wunderground.com/api/Your_Key/planner_MMDDMMDD/q/CA/San_Francisco.json
            string fromDate = weatherInfo.DepartDate.ToString("MMdd");
            string toDate = weatherInfo.ReturnDate.ToString("MMdd");
            string url = string.Format("planner_{0}{1}/q/{2}/{3}.json", fromDate, toDate, weatherInfo.State, weatherInfo.City);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";
            APIResponse result = _apiCaller.Get(url). Result;
   
            if (result.StatusCode == HttpStatusCode.OK)
            {
                HistoryOutput weather = new HistoryOutput();
                weather = ServiceStackSerializer.DeSerialize<HistoryOutput>(result.Response);
                //Mapper.CreateMap<HistoryOutput, TripWeather>()
                //   .ForMember(h => h.TempHighAvg, m => m.MapFrom(s => s.trip.temp_high))
                //   .ForMember(h => h.TempLowAvg, m => m.MapFrom(s => s.trip.temp_low))
                //   .ForMember(h => h.ChanceOf, m => m.MapFrom(s => s.trip.chance_of))
                //   .ForMember(h => h.CloudCover, m => m.MapFrom(s => s.trip.cloud_cover));
                //TripWeather tripWeather = Mapper.Map<HistoryOutput, TripWeather>(weather);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, weather);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response); 
        }
    }
}
