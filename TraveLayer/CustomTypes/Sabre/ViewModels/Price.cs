using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    public class Price
    {
        public LowestFare LowestFare { get; set; }       
        public LowestNonStopFare LowestNonStopFare { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
    }
}
