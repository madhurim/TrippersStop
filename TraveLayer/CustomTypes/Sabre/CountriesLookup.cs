using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
  
    public class OriginCountry
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }

    public class DestinationCountry
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }

   
    public class OTA_CountriesLookup
    {
        public string PointOfSale { get; set; }
        public List<OriginCountry> OriginCountries { get; set; }
        public List<DestinationCountry> DestinationCountries { get; set; }
        public List<Link> Links { get; set; }
    }

    //public class CountriesLookup
    //{
    //    public OTA_CountriesLookup OTA_CountriesLookup { get; set; }
    //}


}


 //    {
 // "PointOfSale": "DE",
 // "OriginCountries":
 // [
 //  {
 //   "CountryCode": "AT",
 //   "CountryName": "Austria"
 //  },
 //  {
 //   "CountryCode": "DE",
 //   "CountryName": "Germany"
 //  },
 //  ...
 // ],
 // "DestinationCountries":
 // [
 //  {
 //   "CountryCode": "AU",
 //   "CountryName": "Australia"
 //  },
 //  ...
 // ],
 // "Links":
 // [
 //  {
 //  "rel": "self",
 //  "href": "https://api.sabre.com/v1/lists/supported/countries?pointofsalecountry=DE"
 // },
 // {
 //  "rel": "linkTemplate",
 //  "href": "https://api.test.sabre.com/v1/lists/supported/countries?pointofsalecountry=<pointofsalecountry>"
 //  }
 // ]
 //}
