using ExpressMapper;
using System.Configuration;
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
using TraveLayer.CustomTypes.TripAdvisor.Request;
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
    public class HotelsController : ApiController
    {

        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        const string HotelsCacheKey = "TripAdvisor.Hotels";

        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public HotelsController(ITripAdvisorAPIAsyncCaller apiCaller)
        {
            _apiCaller = apiCaller;
        }

        private string APIHotelsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorHotelUrl"];
            }
        }

        /// <summary>
        /// The response provides results to a maximum of 10 hotels/accommodations  
        /// </summary>
        //[ResponseType(typeof(TripWeather))]
        [Route("api/tripadvisor/hotels")]
        [TrippismCache(HotelsCacheKey)]
        [HttpGet]
        [ResponseType(typeof(LocationAttraction))]
        public async Task<IHttpActionResult> Get([FromUri]HotelsRequest hotelMapRequest)
        {
            return await Task.Run(() =>
            { return GetHotels(hotelMapRequest); });
        }
        private IHttpActionResult GetHotels(HotelsRequest hotelMapRequest)
        {
            string urlAPI = GetApiURL(hotelMapRequest);
            APIResponse result = _apiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var hotels = ServiceStackSerializer.DeSerialize<LocationInfo>(result.Response);
                var locations = Mapper.Map<LocationInfo, LocationAttraction>(hotels);
                return Ok(locations);
            }
            return ResponseMessage(new HttpResponseMessage(result.StatusCode));
        }

        private string GetApiURL(HotelsRequest hotelsRequest)
        {
            string location = string.Join(",", hotelsRequest.Latitude, hotelsRequest.Longitude);
            StringBuilder apiUrl = new StringBuilder(string.Format(APIHotelsUrl, location));
            apiUrl.Append("?key={0}");
            if (!string.IsNullOrWhiteSpace(hotelsRequest.Locale))
                apiUrl.Append("&lang=" + hotelsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(hotelsRequest.Currency))
                apiUrl.Append("&lang=" + hotelsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(hotelsRequest.LengthUnit))
                apiUrl.Append("&lang=" + hotelsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(hotelsRequest.Distance))
                apiUrl.Append("&lang=" + hotelsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(hotelsRequest.SubCategory))
                apiUrl.Append("&subcategory=" + hotelsRequest.SubCategory);
            return apiUrl.ToString();
        }
    }
}
