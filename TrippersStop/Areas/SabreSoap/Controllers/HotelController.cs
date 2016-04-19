using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TraveLayer.CustomTypes.SabreSoap.ViewModels;
using TraveLayer.SoapServices.Hotel;
using TraveLayer.SoapServices.Hotel.Sabre;
using TraveLayer.SoapServices.Hotel.Sabre.HotelAvailabilityRequest;
using TrippismApi.TraveLayer;

namespace Trippism.Areas.SabreSoap.Controllers
{
    public class HotelController : ApiController
    {

        readonly ISabreHotel _apiCaller;
        readonly ICacheService _cacheService;

        /// <summary>
        /// Set api class and cache service.
        /// </summary>
        public HotelController(ISabreHotel apiCaller, ICacheService cacheService)
        {
            _apiCaller = apiCaller;
            _cacheService = cacheService;
        }

        [Route("api/sabre/hotels")]
        public HttpResponseMessage Get([FromUri]HotelRequest hotelRequest)
        {
            TrippismApi.ApiHelper.SetSabreSoapApiToken(_apiCaller, _cacheService);
            var response = GetResponse(hotelRequest);
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        private HotelOutput GetResponse(HotelRequest hotelRequest)
        {
            var hotels = _apiCaller.GetHotels(hotelRequest);
            HotelOutput hotelResponse = new HotelOutput();

            if (hotels != null && hotels.ApplicationResults != null && hotels.ApplicationResults.status == CompletionCodes.Complete)
            {
                var response = Mapper.Map<OTA_HotelAvailRS, TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels.Hotels>(hotels);
                hotelResponse.ThreeStar = GetHotel(response, "3 CROWN");
                hotelResponse.FourStar = GetHotel(response, "4 CROWN");
                hotelResponse.FiveStar = GetHotel(response, "5 CROWN");             
            }
            return hotelResponse;
        }


        private Hotel GetHotel(TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels.Hotels response,string star)
        {
            Hotel hotel = new Hotel()
            {
                Star = star
            };
            var hotelAvailability= response.HotelAvailability.Where(a => a.HotelDetail.HotelRating != null && a.HotelDetail.HotelRating.Any(p => p.RatingText == star)).ToList();
            if (hotelAvailability != null && hotelAvailability.Count > 0)
            {
                hotel.MinimumPriceAvg = hotelAvailability.Average(a => a.HotelDetail.RateRange != null ? Convert.ToDecimal(a.HotelDetail.RateRange.Min) : 0);
                hotel.MaximumPriceAvg = hotelAvailability.Average(a => a.HotelDetail.RateRange != null ? Convert.ToDecimal(a.HotelDetail.RateRange.Max) : 0);
                hotel.CurrencyCode = hotelAvailability[0].HotelDetail != null && hotelAvailability[0].HotelDetail.RateRange != null ? hotelAvailability[0].HotelDetail.RateRange.CurrencyCode : string.Empty;
            }
            return hotel;
        }
    }
}
