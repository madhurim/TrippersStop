using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre.ViewModel
{
    /// <summary>
    /// Sabre CountriesController response
    /// </summary> 
    public class Countries
    {
        public string PointOfSale { get; set; }
        public List<OriginCountry> OriginCountries { get; set; }
        public List<DestinationCountry> DestinationCountries { get; set; }

    }
}
