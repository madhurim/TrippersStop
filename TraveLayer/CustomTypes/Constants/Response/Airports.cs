using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Constants.Response
{
    public class AirportRoot
    {
        public AirportsRoot AirportsRoot { get; set; }
    }

    public class AirportsRoot
    {
        public List<AirportsDetail> AirportsDetail { get; set; }
    }

    public class AirportsDetail
    {
        public string airport_CityCode { get; set; }
        public string airport_CityName { get; set; }
        public string airport_CountryCode { get; set; }
        public string airport_CountryName { get; set; }
        public string region { get; set; }
        public bool isMultiAirport { get; set; }
        public List<Airport> Airports { get; set; }
    }
    public class Airport
    {
        public string airport_Code { get; set; }
        public string airport_FullName { get; set; }
        public string airport_Lat { get; set; }
        public string airport_Lng { get; set; }
        public string alternatenames { get; set; }
        public string[] themes { get; set; }
    }
}
