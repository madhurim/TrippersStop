using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trippism.Areas.TripAdvisor.Models
{
    public class LocationRequest
    {
        public string Locale { get; set; }
        public string Currency { get; set; }
        [Required]
        public string LocationId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Latitude { get; set; }
        [Required]
        public string Longitude { get; set; }
    }
}