using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace TrippersStop.Areas.Sabre.Models
{
    public class LowFareForecastRequest
    {
         [Required]
        public string Origin { get; set; }
        [Required]
        public string Destination { get; set; }

        [Required]
        public string DepartureDate { get; set; }
        [Required]
        public string ReturnDate { get; set; }

    }
}
