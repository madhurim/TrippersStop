using System.Collections.Generic;
using TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels;

namespace TraveLayer.CustomTypes.SabreSoap.ViewModels
{
    public class HotelInfo
    {
        public List<HotelAvailability> HotelAvailability { get; set; }
        public List<HotelDetail> HotelDetail { get; set; }
        public decimal MinimumPriceAvg { get; set; }
        public decimal MaximumPriceAvg { get; set; }
    }
}
