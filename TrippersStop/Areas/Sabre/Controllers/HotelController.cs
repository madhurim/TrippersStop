using ExpressMapper;
using System.Web.Http;
using System.Web.Http.Description;
using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels;
using TraveLayer.CustomTypes.SabreSoap.ViewModels;
using TrippismApi.TraveLayer;
using TrippismApi.TraveLayer.Hotel.Sabre.HotelAvailabilityRequest;
using TrippismApi.TraveLayer.Hotel.Sabre;
using BusinessLogic;
using System.Threading.Tasks;

namespace TrippismApi.Areas.Sabre.Controllers
{
    public class HotelController : ApiController
    {

        readonly ISabreHotelSoapCaller _apiCaller;
        readonly ICacheService _cacheService;
        readonly IBusinessLayer<Hotels, HotelOutput> _iBusinessLayer;
        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public HotelController(ISabreHotelSoapCaller apiCaller, ICacheService cacheService, IBusinessLayer<Hotels, HotelOutput> iBusinessLayer)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
            _iBusinessLayer = iBusinessLayer;
        }

        /// <summary>
        /// API returns the hoatel range based on city code and date 
        /// </summary>
        [ResponseType(typeof(HotelOutput))]
        [Route("api/sabre/hotels")]
        public async Task<IHttpActionResult> Get([FromUri]HotelRequest hotelRequest)
        {
            return await Task.Run(() =>
             { return GetHotel(hotelRequest); });
        }

        private IHttpActionResult GetHotel(HotelRequest hotelRequest)
        {
            TrippismApi.ApiHelper.SetSabreSoapApiToken(_apiCaller, _cacheService);
            var response = GetResponse(hotelRequest);
            return Ok(response);
        }
        private HotelOutput GetResponse(HotelRequest hotelRequest)
        {
            var hotels = _apiCaller.GetHotels(hotelRequest);
            HotelOutput hotelResponse = new HotelOutput();

            if (hotels != null && hotels.ApplicationResults != null && hotels.ApplicationResults.status == CompletionCodes.Complete)
            {
                var response = Mapper.Map<OTA_HotelAvailRS, Hotels>(hotels);
                hotelResponse = _iBusinessLayer.Process(response);
            }
            return hotelResponse;
        }

        /// <summary>
        /// API returns the basic hotel details based on city code and date 
        /// </summary>
        [ResponseType(typeof(HotelOutput))]
        [Route("api/sabre/hotels/all")]
        public async Task<IHttpActionResult> GetAll([FromUri]HotelRequest hotelRequest)
        {
            return await Task.Run(() =>
            { return GetAllHotel(hotelRequest); });
        }

        private IHttpActionResult GetAllHotel(HotelRequest hotelRequest)
        {
            TrippismApi.ApiHelper.SetSabreSoapApiToken(_apiCaller, _cacheService);
            var response = GetAllResponse(hotelRequest);
            return Ok(response);
        }
        private HotelInfo GetAllResponse(HotelRequest hotelRequest)
        {
            var hotels = _apiCaller.GetHotels(hotelRequest);
            if (hotels != null && hotels.ApplicationResults != null && hotels.ApplicationResults.status == CompletionCodes.Complete)
            {
                var response = Mapper.Map<OTA_HotelAvailRS, HotelInfo>(hotels);
                return response;
            }
            return null;
        }
    }
}
