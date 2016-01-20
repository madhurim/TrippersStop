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
using TraveLayer.CustomTypes.TripAdvisor.Request;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    ///Call the API with the unique ID for a hotel, restaurant, attraction or destination.  
    ///The response provides data such as:  name, address, overall traveler rating, number of reviews, 
    ///link to read all reviews, link to write reviews, recent review snippets, along with additional data elements.  
    ///Some data elements may not output if they do not apply to the particular type of location
    ///This api call is made after a call from TripAdvisor AttractionController , HotelsController or RestaurantsController
    ///These 3 controllers will return a LocationId from TripAdvisor , which will further help us in getting the reviews and rankings from TripAdvisor.
    ///Since TripAdvisor does not return photos, we pass name , lat , long to Google Api to get the photos data + more data from google.
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class LocationController : ApiController
    {
        readonly ITripAdvisorAPIAsyncCaller _tripAdvisorApiCaller;
        readonly IAsyncGoogleAPICaller _googleAPICaller;
        const string LocationCacheKey = "TripAdvisor.Location";
        /// <summary>
        /// Set Api caller and Cache service
        /// </summary>
        public LocationController(ITripAdvisorAPIAsyncCaller tripAdvisorApiCaller, ICacheService cacheService, IAsyncGoogleAPICaller googleAPICaller)
        {
            _tripAdvisorApiCaller = tripAdvisorApiCaller;
            _googleAPICaller= googleAPICaller;
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
        //[TrippismCache(LocationCacheKey)]
        [HttpGet]
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> Get([FromUri]LocationRequest locationRequest)
        {
            return await Task.Run(() =>
            { return GetLocationDetail(locationRequest); });
        }
        private IHttpActionResult GetLocationDetail(LocationRequest locationRequest)
        {
            Location location = null;
            string placeId = string.Empty;
            Parallel.Invoke(
                () => { location = GetTripAdvisorResponse(locationRequest); },
                () => { placeId = GetGoogleResponse(locationRequest); }
              );
            if (location != null)
            {
                location.GooglePlaceId = placeId;
                return Ok(location);
            }
            else
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        private Location GetTripAdvisorResponse(LocationRequest locationRequest)
        {
            string urlAPI = GetApiURL(locationRequest);
            APIResponse result = _tripAdvisorApiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var locationDetail = ServiceStackSerializer.DeSerialize<Datum>(result.Response);
                var location = Mapper.Map<Datum, Location>(locationDetail);
                return location;
            }
            return null;
        }
        private string GetGoogleResponse(LocationRequest locationRequest)
        {
            _googleAPICaller.Accept = "application/json";
            _googleAPICaller.ContentType = "application/json";
            string googleUrl = GetGoogleApiURL(locationRequest);
            APIResponse result = _googleAPICaller.Get(googleUrl).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var googleResponse = ServiceStackSerializer.DeSerialize<TraveLayer.CustomTypes.Google.Response.GoogleOutput>(result.Response);
                if (googleResponse != null && googleResponse.results.Count >0)
               {
                   return (googleResponse.results.FirstOrDefault()).place_id;
               }
            }
            return null;
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

        private string GetGoogleApiURL(LocationRequest locationRequest)
        {
            StringBuilder url = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(locationRequest.Latitude) && !string.IsNullOrWhiteSpace(locationRequest.Longitude))
                url.Append(string.Format("&location={0}", locationRequest.Latitude + "," + locationRequest.Longitude));
            if (!string.IsNullOrWhiteSpace(locationRequest.Name))
            {
                url.Append(string.Format("&name={0}", locationRequest.Name));
                url.Append(string.Format("&keyword={0}", locationRequest.Name));
            }                      
            return url.ToString();
        }
    }
}
