using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.Request
{
    public class InstaFlightSearchInput
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DepartureDate { get; set; }
        public string ReturnDate { get; set; }
        public string IncludedCarriers { get; set; }
        public string PointOfSaleCountry { get; set; }
        public short? outboundflightstops { get; set; }
        public short? inboundflightstops { get; set; }
    }
}
