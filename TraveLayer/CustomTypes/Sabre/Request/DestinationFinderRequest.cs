using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.Request
{
    public class DestinationFinderRequest
    {
        public string Origin { get; set; }
        public string Earliestdeparturedate{ get; set; }
        public string Latestdeparturedate{ get; set; }
        public string Lengthofstay { get; set; }

        public string Theme { get; set; }

    }
}
