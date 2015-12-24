using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{
    public class RankingData
    {
        public string ranking_string { get; set; }
        public string ranking_out_of { get; set; }
        public string geo_location_id { get; set; }
        public string ranking { get; set; }
        public string geo_location_name { get; set; }
    }
}
