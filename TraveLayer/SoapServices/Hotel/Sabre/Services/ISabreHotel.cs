using System.Configuration;
using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TraveLayer.SoapServices.Hotel.Sabre.HotelAvailabilityRequest;
using TraveLayer.SoapServices.Hotel.Sabre.SessionCreateRequest;

namespace TraveLayer.SoapServices.Hotel.Sabre
{
    public interface ISabreHotel 
    {
        OTA_HotelAvailRS GetHotels(HotelRequest hotelRequest);
        string SabreSessionTokenKey{get;}

        string SecurityToken { get; set; }
    }
}
