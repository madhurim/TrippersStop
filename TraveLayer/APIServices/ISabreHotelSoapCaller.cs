using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TrippismApi.TraveLayer.Hotel.Sabre.HotelAvailabilityRequest;

namespace TrippismApi.TraveLayer.Hotel.Sabre
{
    public interface ISabreHotelSoapCaller : ISabreSoapCaller
    {
        OTA_HotelAvailRS GetHotels(HotelRequest hotelRequest);
        string SabreSessionTokenKey{get;}

    }
}
