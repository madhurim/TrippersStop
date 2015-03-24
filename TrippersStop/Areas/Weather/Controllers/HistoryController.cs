using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.Response;
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
        [Route("api/Weather/History")]
        [HttpGet]
        public HttpResponseMessage Get(string latitude, string longitude,DateTime date)
        {
       // http://api.wunderground.com/api/1fc34e46d99dde6d/history_20060405/q/37.776289,-122.395234.json
            string historyDate = date.ToString("yyyyMMdd");

            string url = string.Format("history_{0}/q/{1},{2}.json", historyDate, latitude, longitude);
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
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result.Response);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response); 
        }
    }
}
