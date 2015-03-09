using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TraveLayer.CustomTypes.ViewModel
{
    public class FareForecast
    {
        
        public string Origin { get; set; }
        
        public string Destination { get; set; }
       
        public string EarliestDepartureDate { get; set; }
       
        public string LatestDepartureDate { get; set; }
       
        public string LengthOfStay { get; set; }
    }
}
