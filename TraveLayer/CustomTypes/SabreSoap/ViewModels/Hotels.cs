using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.SoapServices.ViewModels
{
    public class Hotels
    {
        public List<HotelAvailability> HotelAvailability{ get; set; }

        public decimal MinimumPriceAvg { get; set; }
        public decimal MaximumPriceAvg { get; set; }
    }
}
