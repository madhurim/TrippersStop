using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Google.Request;
using TraveLayer.CustomTypes.Google.Response;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq;
using System.Configuration;
using TraveLayer.CustomTypes.TripAdvisor.Request;
using System.Text;
using System.Collections.Generic;
namespace Trippism.Areas.GooglePlace.Controllers
{
    public class GooglePlaceController : ApiController
    {
        const string TrippismKey = "Trippism.GooglePlace.";
        IAsyncGoogleAPICaller _apiCaller;
        ICacheService _cacheService;
        readonly ITripAdvisorAPIAsyncCaller _tripAdvisorApiCaller;

        private string APILocationUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorLocationPropertiesUrl"];
            }
        }
        private string APIAttractionsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorAttractionsUrl"];
            }
        }
        public GooglePlaceController(IAsyncGoogleAPICaller apiCaller, ICacheService cacheService, ITripAdvisorAPIAsyncCaller tripAdvisorApiCaller)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _tripAdvisorApiCaller = tripAdvisorApiCaller;
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Google.ViewModels.Google))]
        [Route("api/googleplace/locationsearch")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]GoogleInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.Latitude, locationsearch.Longitude, locationsearch.Types);
            var tripGooglePlace = _cacheService.GetByKey<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey);
            if (tripGooglePlace != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripGooglePlace);
            }
            return await Task.Run(() =>
            { return GetResponse(locationsearch, cacheKey); });
        }


        /// <summary>
        /// The response provides review comments of attractions
        /// </summary>
        [ResponseType(typeof(TraveLayer.CustomTypes.TripAdvisor.ViewModels.LocationAttraction))]
        [Route("api/googleplace/reviews")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAttractions([FromUri]AttractionsRequest attractionsRequest)
        {
            return await Task.Run(() =>
            { return GetMapAttractions(attractionsRequest); });
        }

        private IHttpActionResult GetMapAttractions(AttractionsRequest attractionsRequest)
        {
            string urlAPI = GetAttractionsApiURL(attractionsRequest);
            APIResponse result = _tripAdvisorApiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var attractions = ServiceStackSerializer.DeSerialize<TraveLayer.CustomTypes.TripAdvisor.Response.LocationInfo>(result.Response);
                if (attractions != null && attractions.data != null && attractions.data.Count > 0)
                {
                    var location = attractions.data.FirstOrDefault();
                    return GetLocationDetail(location.location_id);
                }


            }
            return ResponseMessage(new HttpResponseMessage(result.StatusCode));
        }

        private IHttpActionResult GetLocationDetail(string locationId)
        {
            string urlAPI = GetLocationApiURL(locationId);
            APIResponse result = _tripAdvisorApiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var locationDetail = ServiceStackSerializer.DeSerialize<TraveLayer.CustomTypes.TripAdvisor.Response.Datum>(result.Response);
                var location = ExpressMapper.Mapper.Map<TraveLayer.CustomTypes.TripAdvisor.Response.Datum, TraveLayer.CustomTypes.TripAdvisor.ViewModels.Location>(locationDetail);
                return Ok(location);
            }
            return null;
        }

        private string GetLocationApiURL(string locationId)
        {
            string locationUrl = string.Format(APILocationUrl, locationId) + "?key={0}";
            return locationUrl;
        }
        private string GetAttractionsApiURL(AttractionsRequest attractionsRequest)
        {
            string location = string.Join(",", attractionsRequest.Latitude, attractionsRequest.Longitude);
            StringBuilder apiUrl = new StringBuilder(string.Format(APIAttractionsUrl, location));
            apiUrl.Append("?key={0}");
            if (!string.IsNullOrWhiteSpace(attractionsRequest.Locale))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.Currency))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.LengthUnit))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.Distance))
                apiUrl.Append("&lang=" + attractionsRequest.Locale);
            if (!string.IsNullOrWhiteSpace(attractionsRequest.SubCategory))
                apiUrl.Append("&subcategory=" + attractionsRequest.SubCategory);
            return apiUrl.ToString();
        }

        private HttpResponseMessage GetResponse(GoogleInput locationsearch, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            string url = GetURL(locationsearch);
            //let's just get English language results
            url = url + "&language=en";
            APIResponse result = _apiCaller.Get(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                GoogleOutput googleplace = new GoogleOutput();
                googleplace = ServiceStackSerializer.DeSerialize<GoogleOutput>(result.Response);

                //if next_page_token exists get the next set of results , combine.
                if (!string.IsNullOrWhiteSpace(googleplace.next_page_token))
                {
                    result = _apiCaller.Get(url + "&pagetoken=" + googleplace.next_page_token).Result;
                    GoogleOutput nextResult = ServiceStackSerializer.DeSerialize<GoogleOutput>(result.Response);
                    googleplace.results.AddRange(nextResult.results);
                }

                if (locationsearch.ExcludeTypes != null)
                    googleplace.results = googleplace.results.Where(x => !x.types.Intersect(locationsearch.ExcludeTypes).Any()).ToList();

                //Filter : the types returned by Google should have the types in the request                
                //Keyword=Beach : the name in the result should have "beach" , "resort" or "resorts"
                googleplace = CleanUpGoogleData(locationsearch, googleplace);

                Mapper.CreateMap<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>();
                Mapper.CreateMap<results, TraveLayer.CustomTypes.Google.ViewModels.results>();
                Mapper.CreateMap<Geometry, TraveLayer.CustomTypes.Google.ViewModels.Geometry>();
                Mapper.CreateMap<Location, TraveLayer.CustomTypes.Google.ViewModels.Location>();
                //Mapper.CreateMap<Photos, TraveLayer.CustomTypes.Google.ViewModels.Photos>();

                TraveLayer.CustomTypes.Google.ViewModels.Google lstLocations = Mapper.Map<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>(googleplace);
                if (lstLocations.results.Any())
                    _cacheService.Save<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey, lstLocations);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);

                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
        }
        private static GoogleOutput CleanUpGoogleData(GoogleInput locationsearch, GoogleOutput googleplace)
        {
            List<results> results = new List<results>();

            string[] types = null;

            // &types=restaurant|cafe 
            if (locationsearch.Types.Contains("&types="))
            {
                types = locationsearch.Types.Replace("&types=", System.String.Empty).Split('|');
            }

            if (types != null && types.Length > 0 && !types[0].Contains("keyword"))
            {
                foreach (var item in googleplace.results)
                {
                    var itemTypes = item.types;
                    foreach (string type in types)
                    {
                        if (itemTypes.Any(x => x.ToLower().Contains(type)))
                        {
                            results.Add(item);
                            break;
                        }
                    }
                }
            }

            //&keyword=beach
            if (locationsearch.Types == "&keyword=beach")
            {
                string[] beachKeywords = { "beach", "resort", "resorts" };
                foreach (var item in googleplace.results)
                {
                    foreach (string keyword in beachKeywords)
                    {
                        if (item.name.ToLower().Contains(keyword))
                        {
                            results.Add(item);
                            break;
                        }
                    }
                }
            }

            //Order by rank : The highest ranked first -            
            googleplace.results = results.OrderByDescending(x => x.rating).ToList();
            return googleplace;
        }
        private string GetURL(GoogleInput locationsearch)
        {
            string url = string.Empty;
            if (!string.IsNullOrWhiteSpace(locationsearch.NextPageToken))
                url += string.Format("&pagetoken={0}", locationsearch.NextPageToken);
            if (!string.IsNullOrWhiteSpace(locationsearch.Latitude) && !string.IsNullOrWhiteSpace(locationsearch.Longitude))
                url += string.Format("&location={0}", locationsearch.Latitude + "," + locationsearch.Longitude);
            if (!string.IsNullOrWhiteSpace(locationsearch.Types))
                url += locationsearch.Types;
            if (!string.IsNullOrWhiteSpace(locationsearch.Keywords))
                url += locationsearch.Keywords;
            return url;
        }
    }
}