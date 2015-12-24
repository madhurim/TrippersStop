using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trippism.Areas.TripAdvisor.Models
{
    public class MapRequest
    {
        public string Locale { get; set; }
        public string Currency { get; set; }
        public string LengthUnit { get; set; }
        public string Distance { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
    }
}