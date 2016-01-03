using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.TripAdvisor.Response;
using TraveLayer.CustomTypes.TripAdvisor.ViewModels;
using Trippism.APIExtention.Filters;
using Trippism.Areas.TripAdvisor.Models;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    ///Call the API with the unique ID for a hotel, restaurant, attraction or destination.  The response provides data such as:  name, address, overall traveler rating, number of reviews, link to read all reviews, link to write reviews, recent review snippets, along with additional data elements.  Some data elements may not output if they do not apply to the particular type of location
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class LocationController : ApiController
    {
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        const string LocationCacheKey = "TripAdvisor.Location";
        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public LocationController(ITripAdvisorAPIAsyncCaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
        }

        private string APILocationUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorLocationPropertiesUrl"];
            }
        }

        /// <summary>
        /// The response provides results to location detail based on id  
        /// </summary>
        [Route("api/tripadvisor/location")]
        [TrippismCache(LocationCacheKey)]
        [HttpGet]
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> Get([FromUri]LocationRequest locationRequest)
        {
            return await Task.Run(() =>
            { return GetLocationDetail(locationRequest); });
        }
        private IHttpActionResult GetLocationDetail(LocationRequest locationRequest)
        {
            string urlAPI = GetApiURL(locationRequest);
            APIResponse result = _apiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var locationDetail = ServiceStackSerializer.DeSerialize<Datum>(result.Response);
                var location = Mapper.Map<Datum, Location>(locationDetail);
                return Ok(location);
            }
            return ResponseMessage(new HttpResponseMessage(result.StatusCode));
        }

        private string GetApiURL(LocationRequest locationRequest)
        {
            string location = string.Join(",", locationRequest.LocationId);
            StringBuilder apiUrl = new StringBuilder(string.Format(APILocationUrl, location));
            apiUrl.Append("?key={0}");
            if (!string.IsNullOrWhiteSpace(locationRequest.Locale))
                apiUrl.Append("&lang=" + locationRequest.Locale);
            if (!string.IsNullOrWhiteSpace(locationRequest.Currency))
                apiUrl.Append("&lang=" + locationRequest.Locale);
            return apiUrl.ToString();
        }
    }
}
