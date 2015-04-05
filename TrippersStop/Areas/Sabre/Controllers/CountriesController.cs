using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// Retrieves a list of origin and destination countries.
    /// </summary>
    public class CountriesController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public CountriesController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;        
        }
        // GET api/countries
        /// <summary>
        /// Retrieves a list of origin and destination countries.
        /// </summary>
        [ResponseType(typeof(Countries))]
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/countries";
            return GetResponse(url);
        }
        /// <summary>
        /// Retrieves a list of origin and destination countries based on point of sale country.
        /// </summary>
        [ResponseType(typeof(Countries))]
        public HttpResponseMessage Get(string pointofsalecountry)
        {
            string url = string.Format("v1/lists/supported/countries?pointofsalecountry={0}", pointofsalecountry);
            return GetResponse(url);
        }
        /// <summary>
        /// Get response from api based on url.
        /// </summary>
        private HttpResponseMessage GetResponse(string url)
        {
            SabreApiTokenHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                SabreApiTokenHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                OTA_CountriesLookup cities = new OTA_CountriesLookup();
                cities = ServiceStackSerializer.DeSerialize<OTA_CountriesLookup>(result.Response);
                Mapper.CreateMap<OTA_CountriesLookup, Countries>();
                Countries countries = Mapper.Map<OTA_CountriesLookup, Countries>(cities);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, countries);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
