using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TrippersStop.Areas.Sabre.Models
{
    public class FareForecastRequest
    {
         [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public string EarliestDepartureDate { get; set; }
        [Required]
        public string LatestDepartureDate { get; set; }
        [Required]
        public string LengthOfStay { get; set; }
    }
}
