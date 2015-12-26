using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippism.Areas.TripAdvisor.Models
{
    public class RestaurantMapRequest : HotelMapRequest 
    {
        public string Cuisines { get; set; }
        public string Prices { get; set; }
    }
}