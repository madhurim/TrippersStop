using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.TripAdvisor.Response
{

    public class RootObject
    {
        public List<Datum> data { get; set; }
        public Paging paging { get; set; }
    }

}
