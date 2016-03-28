using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{

    public class Ancestor
    {
        public string abbrv { get; set; }
        public string level { get; set; }
        public string name { get; set; }
        public string location_id { get; set; }
    }
}
