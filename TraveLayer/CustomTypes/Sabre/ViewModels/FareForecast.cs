using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    /// <summary>
    /// Sabre FareForecastController Request
    /// </summary> 
    public class FareForecast
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string EarliestDepartureDate { get; set; }
        public string LatestDepartureDate { get; set; }
        public string LengthOfStay { get; set; }
    }
}
