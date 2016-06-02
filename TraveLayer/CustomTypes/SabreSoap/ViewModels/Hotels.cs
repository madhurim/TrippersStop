using System.Collections.Generic;

namespace TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels
{
    public class Hotels
    {
        public List<HotelAvailability> HotelAvailability{ get; set; }

        public decimal MinimumPriceAvg { get; set; }
        public decimal MaximumPriceAvg { get; set; }
    }
}
