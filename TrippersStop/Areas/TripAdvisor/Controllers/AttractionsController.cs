﻿using ExpressMapper;
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
using BusinessLogic;

namespace Trippism.Areas.TripAdvisor.Controllers
{
    /// <summary>
    /// When specifying a Lat/Long point, returns a list of 10 properties found within a given distance from that point. If there are more than 10 properties within the radius requested, the 10 nearest properties will be returned.   In lieu of lat long, can specify a location ID and the output will return nearest POIs
    /// </summary>
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class AttractionsController : ApiController
    {

        readonly ITripAdvisorAPIAsyncCaller _apiCaller;
        const string PropertiesCacheKey = "TripAdvisor.Properties";
        const string AttractionsCacheKey = "TripAdvisor.Attractions";
        readonly ITripAdvisorBusinessLayer<LocationAttraction, LocationAttraction> _iTripAdvisorBusinessLayer;
        readonly ITripAdvisorHistoricalBusinessLayer<LocationAttraction, LocationAttraction> _iTripAdvisorHistoricalBusinessLayer;
        readonly ITripAdvisorShopsAndSpasBusinessLayer<LocationAttraction, LocationAttraction> _iTripAdvisorShopsAndSpasBusinessLayer;

        /// <summary>
        /// Set Api caller service
        /// </summary>
        public AttractionsController(ITripAdvisorAPIAsyncCaller apiCaller, ITripAdvisorBusinessLayer<LocationAttraction, LocationAttraction> iTripAdvisorBusinessLayer, ITripAdvisorHistoricalBusinessLayer<LocationAttraction, LocationAttraction> iTripAdvisorHistoricalBusinessLayer, ITripAdvisorShopsAndSpasBusinessLayer<LocationAttraction, LocationAttraction> iTripAdvisorShopsAndSpasBusinessLayer)
        {
            _apiCaller = apiCaller;
            _iTripAdvisorBusinessLayer = iTripAdvisorBusinessLayer;
            _iTripAdvisorHistoricalBusinessLayer = iTripAdvisorHistoricalBusinessLayer;
            _iTripAdvisorShopsAndSpasBusinessLayer = iTripAdvisorShopsAndSpasBusinessLayer;
        }
        private string APIPropertiesUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorPropertiesUrl"];
            }
        }

        private string APIAttractionsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["TripAdvisorAttractionsUrl"];
            }
        }


        /// <summary>
        /// The response provides available properties . Locations are properties in TripAdvisor. Attractions simply represent a
        /// subcategory of properties. 
        /// </summary>
        [Route("api/tripadvisor/properties")]
        [HttpGet]
        [TrippismCache(PropertiesCacheKey)]
        [ResponseType(typeof(LocationAttraction))]
        public async Task<IHttpActionResult> GetProperties([FromUri]PropertiesRequest propertiesRequest)
        {
            return await Task.Run(() =>
            { return GetTripAdvisorProperties(propertiesRequest); });
        }

        /// <summary>
        /// The response provides top 10 attractions . To get attractions you provide a subcategory of properties.
        /// </summary>
        [ResponseType(typeof(LocationAttraction))]
        [Route("api/tripadvisor/attractions")]
        [HttpGet]
        [TrippismCache(AttractionsCacheKey)]
        public async Task<IHttpActionResult> GetAttractions([FromUri]AttractionsRequest attractionsRequest)
        {
            return await Task.Run(() =>
            { return GetMapAttractions(attractionsRequest); });
        }

        private IHttpActionResult GetMapAttractions(AttractionsRequest attractionsRequest)
        {
            string[] SubCategories = null;
            if (!string.IsNullOrWhiteSpace(attractionsRequest.SubCategory))
            {
                SubCategories = attractionsRequest.SubCategory.Split(',');
            }
            if (SubCategories.Length == 1)
            {
                string urlAPI = GetAttractionsApiURL(attractionsRequest);
                APIResponse result = _apiCaller.Get(urlAPI).Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    var attractions = ServiceStackSerializer.DeSerialize<LocationInfo>(result.Response);
                    var locations = Mapper.Map<LocationInfo, LocationAttraction>(attractions);
                    switch (attractionsRequest.SubCategory) {
                        case "Shopping": locations = _iTripAdvisorShopsAndSpasBusinessLayer.Process(locations); break;
                        default: locations = _iTripAdvisorBusinessLayer.Process(locations); break;
                    }                    
                    return Ok(locations);
                }
                return ResponseMessage(new HttpResponseMessage(result.StatusCode));
            }
            else
            {
                var location = new LocationAttraction();
                location.Attractions = new System.Collections.Generic.List<Attraction>();
                Parallel.ForEach(SubCategories,
                   (item) =>
                   {
                       attractionsRequest.SubCategory = item;
                       var result = _apiCaller.Get(GetAttractionsApiURL(attractionsRequest)).Result;
                       if (result.StatusCode == HttpStatusCode.OK)
                       {
                           var attractions = ServiceStackSerializer.DeSerialize<LocationInfo>(result.Response);
                           var locations = Mapper.Map<LocationInfo, LocationAttraction>(attractions);
                           location.Attractions.AddRange(locations.Attractions);
                       }
                   }
                 );
                location = _iTripAdvisorHistoricalBusinessLayer.Process(location);
                return Ok(location);
            }
        }


        private IHttpActionResult GetTripAdvisorProperties(PropertiesRequest propertiesRequest)
        {
            string urlAPI = GetApiURL(propertiesRequest);
            APIResponse result = _apiCaller.Get(urlAPI).Result;
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var properties = ServiceStackSerializer.DeSerialize<LocationInfo>(result.Response);
                var locations = Mapper.Map<LocationInfo, LocationAttraction>(properties);
                return Ok(locations);
            }
            return ResponseMessage(new HttpResponseMessage(result.StatusCode));
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
        private string GetApiURL(PropertiesRequest propertiesRequest)
        {
            string location = string.Join(",", propertiesRequest.Latitude, propertiesRequest.Longitude);
            StringBuilder apiUrl = new StringBuilder(string.Format(APIPropertiesUrl, location));
            apiUrl.Append("?key={0}");
            if (!string.IsNullOrWhiteSpace(propertiesRequest.Locale))
                apiUrl.Append("&lang=" + propertiesRequest.Locale);
            if (!string.IsNullOrWhiteSpace(propertiesRequest.Currency))
                apiUrl.Append("&lang=" + propertiesRequest.Locale);
            if (!string.IsNullOrWhiteSpace(propertiesRequest.LengthUnit))
                apiUrl.Append("&lang=" + propertiesRequest.Locale);
            if (!string.IsNullOrWhiteSpace(propertiesRequest.Distance))
                apiUrl.Append("&lang=" + propertiesRequest.Locale);
            return apiUrl.ToString();
        }
    }
}
