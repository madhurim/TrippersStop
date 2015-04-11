using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TrippersStop.TraveLayer;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class SaleCountryController : ApiController
    {
        IAsyncSabreAPICaller apiCaller;
        public SaleCountryController(IAsyncSabreAPICaller repository)
        {
            apiCaller = repository;
            apiCaller.Accept = "application/json";
            apiCaller.ContentType = "application/x-www-form-urlencoded";
            string token = apiCaller.GetToken().Result;
            apiCaller.Authorization = "bearer";
            apiCaller.ContentType = "application/json";
        }
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/pointofsalecountries";
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            String result = apiCaller.Get(url).Result;
            OTA_PointofSaleCountryCodeLookup countryCodes = new OTA_PointofSaleCountryCodeLookup();
            countryCodes = ServiceStackSerializer.DeSerialize<OTA_PointofSaleCountryCodeLookup>(result);
            Mapper.CreateMap<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>();
            PointofSaleCountryCode pointofSaleCountryCode = Mapper.Map<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>(countryCodes);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pointofSaleCountryCode); 
            return response;
        }
    }
}
