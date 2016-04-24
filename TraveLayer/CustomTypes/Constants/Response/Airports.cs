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
        //public string airport_CityCode { get; set; }
        //public string airport_CityName { get; set; }
        //public string airport_CountryCode { get; set; }
        //public string airport_CountryName { get; set; }
        //public string region { get; set; }
        //public bool isMultiAirport { get; set; }
        //public int? rank { get; set; }
        //public List<Airport> Airports { get; set; }

        public string cCode { get; set; }   // airport_CityCode
        public string cName { get; set; }   // airport_CityName
        public string coCode { get; set; } // airport_CountryCode
        public string coName { get; set; }  // airport_CountryName
        public bool isMAC { get; set; } // isMultiAirport
        public string region { get; set; }
        public int? rank { get; set; }
        public List<Airport> Airports { get; set; }
    }
    public class Airport
    {
        //public string airport_Code { get; set; }
        //public string airport_FullName { get; set; }
        //public string airport_Lat { get; set; }
        //public string airport_Lng { get; set; }
        //public string alternatenames { get; set; }
        //public string[] themes { get; set; }
        //public int? rank { get; set; }
        public string code { get; set; }    // airport_Code
        public string name { get; set; } // airport_FullName
        public string lat { get; set; } //airport_Lat
        public string lng { get; set; } //airport_Lng
        public string names { get; set; } // alternatenames
        public int? rank { get; set; }
        public List<string> themes { get; set; }
    }
    public class AirportCurrency
    {
        public string aCode { get; set; }    // airport_Code
        public string cCode { get; set; }   // Currency_Code
    }
    public class AirportCurrencyOutput
    {
        public List<AirportCurrency> AirportCurrencies { get; set; }   

    }
}
