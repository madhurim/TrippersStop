using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModels
{
    /// <summary>
    /// Sabre CityPairsController response
    /// </summary> 
    public class CityPairs
    {
        public List<OriginDestinationLocation> OriginDestinationLocations { get; set; }
    }
}
