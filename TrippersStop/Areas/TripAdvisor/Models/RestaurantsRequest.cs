using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippism.Areas.TripAdvisor.Models
{
    public class RestaurantsRequest : PropertiesRequest 
    {
        public string SubCategory { get; set; }
        public string Cuisines { get; set; }
        public string Prices { get; set; }
    }
}