using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{
    public class Subrating
    {
        public string rating_image_url { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string localized_name { get; set; }
    }
}
