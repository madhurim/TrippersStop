using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{


    public class Award
    {
        public string award_type { get; set; }
        public string year { get; set; }
        public Images images { get; set; }
        public List<object> categories { get; set; }
        public string display_name { get; set; }
    }
}
