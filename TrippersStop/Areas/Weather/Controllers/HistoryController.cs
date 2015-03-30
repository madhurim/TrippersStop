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
        /// Set api class and cache service.
        /// </summary>
        public HistoryController(IAsyncWeatherAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;           
        }
        /// <summary>
        /// Retrieve city pairs that can be passed to applicable Air Shopping
        /// </summary>
        //[ResponseType(typeof(CityPairs))] 
        [Route("api/weather/history")]
        [HttpGet]
        public HttpResponseMessage Get([FromUri]WeatherInfo weatherInfo)
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
                WeatherFeatures weather = new WeatherFeatures();
                weather = ServiceStackSerializer.DeSerialize<WeatherFeatures>(result.Response);
                //Mapper.CreateMap<OTA_ThemeAirportLookup, ThemeAirport>();
                //ThemeAirport themeAirport = Mapper.Map<OTA_ThemeAirportLookup, ThemeAirport>(themeAirportLookup);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, weather);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response); 
        }
    }
}
