using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{
    public class Paging
    {
        public object next { get; set; }
        public object previous { get; set; }
        public string results { get; set; }
        public string total_results { get; set; }
        public string skipped { get; set; }
    }
}
