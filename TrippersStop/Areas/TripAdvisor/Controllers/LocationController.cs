using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    /// The response provides data such as:  name, address, overall traveler rating, number of reviews, link to read all reviews, link to write reviews, recent review snippets, along with additional data elements.
    /// </summary>
    public class LocationController : ApiController
    {
        const string TrippismKey = "Trippism.TripAdvisor.";
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        readonly ICacheService _cacheService;

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public LocationController(ITripAdvisorAPIAsyncCaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        /// <summary>
        /// The response provides data for all location
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/location")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            return await Task.Run(() =>
           { return GetLocations(); });
        }

        /// <summary>
        ///The response provides data for location based on Locale
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/location")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLocationByLang(string lang)
        {
            return await Task.Run(() =>
            { return GetLocationByLocale(lang); });
        }

        /// <summary>
        ///The response provides data for location based on CurrencyCode
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/location")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLocationByCurrencyCode(string currency)
        {
            return await Task.Run(() =>
            { return GetLocationByCurrency(currency); });
        }

        private IHttpActionResult GetLocationByCurrency(string currency)
        {
            throw new NotImplementedException();
        }
        private IHttpActionResult GetLocationByLocale(string lang)
        {
            throw new NotImplementedException();
        }


        private IHttpActionResult GetLocations()
        {
            return Ok("TBD");
        }
    }
}
