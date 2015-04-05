using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.Response;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippismApi.TraveLayer;

namespace TrippismApi.Areas.Sabre.Controllers
{
    /// <summary>
    /// To retrieve a list of supported point of sale country codes. 
    /// To obtain country codes to use as a request parameter for other REST APIs.
    /// </summary>
    public class SaleCountryController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public SaleCountryController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;   
        }
        /// <summary>
        /// Retrieves a list of supported point of sale country codes and associated country names
        /// </summary>
        [ResponseType(typeof(PointofSaleCountryCode))]
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/pointofsalecountries";
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
                OTA_PointofSaleCountryCodeLookup countryCodes = new OTA_PointofSaleCountryCodeLookup();
                countryCodes = ServiceStackSerializer.DeSerialize<OTA_PointofSaleCountryCodeLookup>(result.Response);
                Mapper.CreateMap<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>();
                PointofSaleCountryCode pointofSaleCountryCode = Mapper.Map<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>(countryCodes);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pointofSaleCountryCode);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
