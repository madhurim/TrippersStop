using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TraveLayer.CustomTypes.ViewModel
{
    public class LowFareForecast
    {
        
        public string Origin { get; set; }
    
        public string Destination { get; set; }

      
        public string DepartureDate { get; set; }
 
        public string ReturnDate { get; set; }

    }
}
