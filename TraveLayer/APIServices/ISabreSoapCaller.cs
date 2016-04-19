using TraveLayer.CustomTypes.Sabre.SoapServices.Request;
using TrippismApi.TraveLayer.Hotel.Sabre.HotelAvailabilityRequest;

namespace TrippismApi.TraveLayer.Hotel.Sabre
{
    public interface ISabreSoapCaller 
    {
        string SabreSoapBaseServiceURL
        {
            get;
        }
        string IPCC
        {
            get;
        }
        string SecurityToken
        {
            get;
            set;
        }
    }
}
