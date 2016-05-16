using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels;
using TraveLayer.CustomTypes.SabreSoap.ViewModels;


namespace BusinessLogic
{
    public class SabreHotelSoapCallerBusinessLayer : IBusinessLayer<Hotels, HotelOutput>
    {
        public HotelOutput Process(Hotels hotels)
        {
            HotelOutput hotelOutput = new HotelOutput();
            hotelOutput.ThreeStar = GetHotel(hotels, "3 CROWN");
            hotelOutput.FourStar = GetHotel(hotels, "4 CROWN");
            hotelOutput.FiveStar = GetHotel(hotels, "5 CROWN");
            return hotelOutput;
        }
        private Hotel GetHotel(Hotels response, string star)
        {
            Hotel hotel = new Hotel()
            {
                Star = star
            };
            var hotelAvailability = response.HotelAvailability.Where(a => a.HotelDetail.HotelRating != null && a.HotelDetail.HotelRating.Any(p => p.RatingText == star)).ToList();
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
