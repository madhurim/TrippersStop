﻿using ExpressMapper;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using Trippism.APIExtention.Filters;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// API to retrieve data for multiple major airports, as opposed to data for a single airport code
    /// </summary>
    [GZipCompressionFilter]
    public class AirportsAtCitiesController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        public string SabreAirportsAtCitiesUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreAirportsAtCitiesUrl"];
            }
        }
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public AirportsAtCitiesController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }
        /// <summary>
        ///  Retrieves the airport, rail station and other codes associated with the MAC code
        /// </summary>
        [ResponseType(typeof(AirportsAtCities))]
        public HttpResponseMessage Get(string city)
        {
            string url = string.Format(SabreAirportsAtCitiesUrl, city);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_AirportsAtCitiesLookup airportsAtCities = new OTA_AirportsAtCitiesLookup();
                airportsAtCities = ServiceStackSerializer.DeSerialize<OTA_AirportsAtCitiesLookup>(result.Response);

                AirportsAtCities airports = Mapper.Map<OTA_AirportsAtCitiesLookup, AirportsAtCities>(airportsAtCities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, airports);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
