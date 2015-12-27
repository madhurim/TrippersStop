using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trippism.Areas.TripAdvisor.Models
{
    public class HotelsRequest : PropertiesRequest
    {
        public string SubCategory { get; set; }
    }
}