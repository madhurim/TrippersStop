using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Trippism.APIExtention.Filters;
using TraveLayer.CustomTypes.Sabre;
using TrippismApi.TraveLayer;
using System.Configuration;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.Request;
using System.Text;
using TrippismApi;
using TraveLayer.CustomTypes.Sabre.Response;
using ExpressMapper;


namespace Trippism.Areas.Sabre.Controllers
{
    [GZipCompressionFilter]
    public class InstaFlightSearchController : ApiController
    {
        IAsyncSabreAPICaller _apiCaller;
        ICacheService _cacheService;

        public string SabreInstaFlightUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SabreInstaFlightUrl"];
            }
        }

        public InstaFlightSearchController(IAsyncSabreAPICaller apiCaller,  ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

         [Route("api/instaflight/search")]
         [Route("api/sabre/instaflight/search")]
        [ResponseType(typeof(InstaFlightSearch))]
        public async Task<HttpResponseMessage> Get([FromUri]InstaFlightSearchInput instaflightRequest)
        {
            string url = GetURL(instaflightRequest);
            return await Task.Run(() =>
            { return GetResponse(url); });
        }

        private string GetURL(InstaFlightSearchInput instaflightRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append(SabreInstaFlightUrl + "?");
            if (!string.IsNullOrWhiteSpace(instaflightRequest.Origin))
            {
                url.Append("origin=" + instaflightRequest.Origin);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.Destination))
            {
                url.Append("&destination=" + instaflightRequest.Destination);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.DepartureDate))
            {
                url.Append("&departuredate=" + instaflightRequest.DepartureDate);
            }
            if (!string.IsNullOrWhiteSpace(instaflightRequest.ReturnDate))
            {
                url.Append("&returndate=" + instaflightRequest.ReturnDate);
            }

         
            return url.ToString();
        }

        private HttpResponseMessage GetResponse(string url, int count = 0)
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
                InstaFlightsSearchOutput flights = new InstaFlightsSearchOutput();
                flights = ServiceStackSerializer.DeSerialize<InstaFlightsSearchOutput>(result.Response);
                InstaFlightSearch fares = Mapper.Map<InstaFlightsSearchOutput, InstaFlightSearch>(flights);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, fares);
                return response;
            }
            return Request.CreateResponse(result.StatusCode, result.Response);
        }
    }
}
