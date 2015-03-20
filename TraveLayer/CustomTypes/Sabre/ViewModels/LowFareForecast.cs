using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModel
{
    /// <summary>
    /// Sabre LowFareForecast response
    /// </summary> 
    public class LowFareForecast
    {
        public string OriginLocation { get; set; }
        public string DestinationLocation { get; set; }
        public string DepartureDateTime { get; set; }
        public string ReturnDateTime { get; set; }
        public Forecast Forecast { get; set; }
        public string Recommendation { get; set; }
        public double LowestFare { get; set; }
        public string CurrencyCode { get; set; }
 
    }
}
