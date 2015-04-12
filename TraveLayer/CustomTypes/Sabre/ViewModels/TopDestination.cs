using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    public class TopDestination
    {
        public string OriginLocation { get; set; }
        public List<Origin> Destinations { get; set; }
        public int LookBackWeeks { get; set; }
    }
}
