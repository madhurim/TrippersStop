using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    /// <summary>
    /// Sabre DestinationsController response
    /// </summary> 
    public class Fares
    {
        public string OriginLocation { get; set; }
        public List<FareInfo> FareInfo { get; set; }
    }
}
