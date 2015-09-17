using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Google.Request;
using TraveLayer.CustomTypes.Google.Response;
using TraveLayer.CustomTypes.Sabre.Response;
using TrippismApi.TraveLayer;
using System.Linq;
using TraveLayer.CustomTypes.Google.ViewModels;
namespace Trippism.Areas.Google.Controllers
{
    public class GoogleGeoCodeController : ApiController
    {
        const string TrippismKey = "Trippism.GoogleGeo.";
        IAsyncGoogleReverseLookupAPICaller _apiCaller;
        ICacheService _cacheService;

        public GoogleGeoCodeController(IAsyncGoogleReverseLookupAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        [ResponseType(typeof(TraveLayer.CustomTypes.Google.ViewModels.GoogleReverseLookup))]
        [HttpGet]
        public async Task<GoogleReverseLookup> Get([FromUri]GoogleInput locationsearch)
        {
            string cacheKey = TrippismKey + string.Join(".", locationsearch.Latitude, locationsearch.Longitude);
            var tripGooglePlace = _cacheService.GetByKey<TraveLayer.CustomTypes.Google.ViewModels.GoogleReverseLookup>(cacheKey);
            if (tripGooglePlace != null)
                return tripGooglePlace;

            return await Task.Run(() =>
            { return GetResponse(locationsearch.Latitude, locationsearch.Longitude, cacheKey); });
        }

        private GoogleReverseLookup GetResponse(string Latitude, string Longitude, string cacheKey)
        {
            _apiCaller.Accept = "application/json";
            _apiCaller.ContentType = "application/json";

            string strLatLongQuery = string.Format("&latlng={0},{1}", Latitude, Longitude);
            APIResponse result = _apiCaller.Get(strLatLongQuery).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                GoogleReverseLookupOutput googleplace = new GoogleReverseLookupOutput();
                googleplace = ServiceStackSerializer.DeSerialize<GoogleReverseLookupOutput>(result.Response);
                if (googleplace != null && googleplace.results != null)
                {
                    var addressComponentList = googleplace.results.Select(x => x.address_components).ToList();
                    var addressComponent = addressComponentList.FirstOrDefault();
                    if (addressComponentList != null)
                    {
                        GoogleReverseLookup googleReverseLookup = addressComponent.Where(x => x.types[0] == "administrative_area_level_1").Select(x => new GoogleReverseLookup() { StateName = x.long_name, StateCode = x.short_name }).FirstOrDefault();
                        if (googleReverseLookup != null)
                        {
                            _cacheService.Save<GoogleReverseLookup>(cacheKey, googleReverseLookup);
                            return googleReverseLookup;
                        }
                    }
                }
            }
            return new GoogleReverseLookup();
        }
    }
}
