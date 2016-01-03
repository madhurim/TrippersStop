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
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.TripAdvisor.Response;
using TraveLayer.CustomTypes.TripAdvisor.ViewModels;
using Trippism.APIExtention.Filters;
using Trippism.Areas.TripAdvisor.Models;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    /// The response provides Hotel data for all location
    /// The desired language locale
    /// Currency code to use for request and response 
    /// Length unit (either 'mi' or 'km') which overrides the default length units used in input and output
    /// Distance in miles (unless another unit is specified using lunit) defining thewidth and height of the bounding rectangle around the center when specified. Defaults to 10 miles. The maximum is 25 miles or 50 kilometers.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class LocationController : ApiController
    {
        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
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
        /// The response provides results to a maximum of 10 hotels/accommodations  
        /// </summary>
        [Route("api/tripadvisor/location")]
        [HttpGet]
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
