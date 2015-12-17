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
namespace Trippism.Areas.GooglePlace.Controllers
{
    public class GooglePlaceController : ApiController
    {
        const string TrippismKey = "Trippism.GooglePlace.";
        IAsyncGoogleAPICaller _apiCaller;
        ICacheService _cacheService;

        public GooglePlaceController(IAsyncGoogleAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Google.ViewModels.Google))]
        [Route("api/googleplace/locationsearch")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]GoogleInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.Latitude, locationsearch.Longitude, locationsearch.Types, locationsearch.Keywords);
            var tripGooglePlace = _cacheService.GetByKey<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey);
            if (tripGooglePlace != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, tripGooglePlace);
            }
            return await Task.Run(() =>
            { return GetResponse(locationsearch, cacheKey); });
        }

        private HttpResponseMessage GetResponse(GoogleInput locationsearch, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            //string strLatitudeandsLongitude = Latitude + "," + Longitude;
            string url = GetURL(locationsearch);
            APIResponse result = _apiCaller.Get(url).Result;

            if (result.StatusCode == HttpStatusCode.OK)
            {
                GoogleOutput googleplace = new GoogleOutput();
                googleplace = ServiceStackSerializer.DeSerialize<GoogleOutput>(result.Response);
                if (locationsearch.ExcludeTypes != null)
                    googleplace.results = googleplace.results.Where(x => !x.types.Intersect(locationsearch.ExcludeTypes).Any()).ToList();

                Mapper.CreateMap<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>();
                Mapper.CreateMap<results, TraveLayer.CustomTypes.Google.ViewModels.results>();
                Mapper.CreateMap<Geometry, TraveLayer.CustomTypes.Google.ViewModels.Geometry>();
                Mapper.CreateMap<Location, TraveLayer.CustomTypes.Google.ViewModels.Location>();
                //Mapper.CreateMap<Photos, TraveLayer.CustomTypes.Google.ViewModels.Photos>();

                TraveLayer.CustomTypes.Google.ViewModels.Google lstLocations = Mapper.Map<GoogleOutput, TraveLayer.CustomTypes.Google.ViewModels.Google>(googleplace);

                if (string.IsNullOrWhiteSpace(locationsearch.NextPageToken))
                    _cacheService.Save<TraveLayer.CustomTypes.Google.ViewModels.Google>(cacheKey, lstLocations);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, lstLocations);

                return response;
            }

            return Request.CreateResponse(result.StatusCode, result.Response);
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
                url += string.Format("&keyword={0}", locationsearch.Keywords);
            return url;
        }
    }
}