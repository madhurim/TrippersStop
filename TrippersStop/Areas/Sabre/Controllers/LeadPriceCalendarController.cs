using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trippism.APIExtention.Filters;
using Trippism.APIHelper;
using TrippismApi.TraveLayer;
using TraveLayer.CustomTypes.Sabre;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using TraveLayer.CustomTypes.Sabre.Request;
using TrippismApi;
using TraveLayer.CustomTypes.Sabre.Response;
using System.Web.Http.Description;
using System.Threading.Tasks;
using ExpressMapper;

namespace Trippism.Areas.Sabre.Controllers
{
    [GZipCompressionFilter]
    [ServiceStackFormatterConfigAttribute]
    public class LeadPriceCalendarController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        
        ICacheService _cacheService;
        const string _leadpriceKey = "Trippism.LeadPrice";
        string _expireTime = ConfigurationManager.AppSettings["RedisExpireInMin"].ToString();

        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public LeadPriceCalendarController(IAsyncSabreAPICaller apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            
        }
        private string GetURL(LeadPrIceCalendarInput leadPriceRequest)
        {
            System.Text.StringBuilder url = new System.Text.StringBuilder();
            url.Append(SabreLeadPriceUrl + "?");
            if (!string.IsNullOrWhiteSpace(leadPriceRequest.Origin))
                url.Append("origin=" + leadPriceRequest.Origin);
            if (!string.IsNullOrWhiteSpace(leadPriceRequest.Destination))
                url.Append("&destination=" + leadPriceRequest.Destination);
            if (!string.IsNullOrWhiteSpace(leadPriceRequest.DepartureDate))
                url.Append("&departuredate=" + leadPriceRequest.DepartureDate);
            if (!string.IsNullOrWhiteSpace(leadPriceRequest.ReturnDate))
                url.Append("&returndate=" + leadPriceRequest.ReturnDate);
            if (!string.IsNullOrWhiteSpace(leadPriceRequest.PointOfSaleCountry))
                url.Append("&pointofsalecountry=" + leadPriceRequest.PointOfSaleCountry);
            if (leadPriceRequest.LengthOfStay > 0)
                url.Append("&lengthofstay=" + leadPriceRequest.LengthOfStay);
           
            return url.ToString();
        }
        private HttpResponseMessage GetResponse(string url, int count = 0)
        {
            //TrippismNLog.SaveNLogData(url);
            ApiHelper.SetApiToken(_apiCaller, _cacheService);
            APIResponse result = _apiCaller.Get(url).Result;

            //string posResponse = "Parameter 'pointofsalecountry' has an unsupported value";
            //if (result.StatusCode == HttpStatusCode.BadRequest && result.Response.ToString() == posResponse)
            //{
            //    APIResponse supportedPOSCountries = GetAPIResponse(SabreLeadPriceUrl);
            //    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, supportedPOSCountries.Response);
            //    return response;
            //}

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                ApiHelper.RefreshApiToken(_cacheService, _apiCaller);
                result = _apiCaller.Get(url).Result;
            }
            if (result.StatusCode == HttpStatusCode.OK)
            {
                LeadPriceCalendarOutput prices = new LeadPriceCalendarOutput();
                prices = ServiceStackSerializer.DeSerialize<LeadPriceCalendarOutput>(result.Response);
                LeadFare fares = new LeadFare();
                fares.DestinationLocation = prices.DestinationLocation;
                fares.OriginLocation = prices.DestinationLocation;
                fares.CurrencyCode = prices.FareInfo[0].CurrencyCode;
                fares.Price = new List<Price>();
                foreach( FareInfo info in prices.FareInfo)
                {
                    Price thisprice = new Price();
                    thisprice.LowestFare = info.LowestFare;
                    thisprice.LowestNonStopFare = info.LowestNonStopFare;
                    thisprice.DepartureDateTime = info.DepartureDateTime;
                    thisprice.ReturnDateTime = info.ReturnDateTime;
                    fares.Price.Add(thisprice);
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                //TrippismNLog.SaveNLogData(result.Response);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
        // GET api/LeadPriceCalendar
        /// <summary>
        /// Returns the current nonstop lead fare and an overall lead fare available to destinations from a specific origin on roundtrip travel dates
        /// for 192 days
        /// </summary>
        /// <param name="leadPriceRequest">
        /// Return record based on destinations Request Type</param>
        [ResponseType(typeof(LeadPriceCalendarOutput))]
        public async Task<HttpResponseMessage> Get([FromUri]LeadPrIceCalendarInput leadPriceRequest)
        {
            string url = GetURL(leadPriceRequest);
            return await Task.Run(() =>
            { return GetResponse(url); });
        }
        
        public string SabreLeadPriceUrl 
        {  
            get
            {
                return ConfigurationManager.AppSettings["SabreLeadPriceUrl"];
            }
        
        }
    }
}
