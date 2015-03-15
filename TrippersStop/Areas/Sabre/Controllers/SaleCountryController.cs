using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModel;
using TrippersStop.Helper;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class SaleCountryController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string url = "v1/lists/supported/pointofsalecountries";
            return GetResponse(url);
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            OTA_PointofSaleCountryCodeLookup countryCodes = new OTA_PointofSaleCountryCodeLookup();
            countryCodes = ServiceStackSerializer.DeSerialize<OTA_PointofSaleCountryCodeLookup>(result);
            Mapper.CreateMap<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>();
            PointofSaleCountryCode pointofSaleCountryCode = Mapper.Map<OTA_PointofSaleCountryCodeLookup, PointofSaleCountryCode>(countryCodes);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, pointofSaleCountryCode); 
            return response;
        }
    }
}
