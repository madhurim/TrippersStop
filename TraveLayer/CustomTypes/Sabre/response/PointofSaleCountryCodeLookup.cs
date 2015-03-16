using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public List<Link> Links { get; set; }
    }

   
    public class OTA_PointofSaleCountryCodeLookup
    {
        public List<Country> Countries { get; set; }
        //public List<Link2> Links { get; set; }
    }

    public class PointofSaleCountryCodeLookup
    {
        public OTA_PointofSaleCountryCodeLookup OTA_PointofSaleCountryCodeLookup { get; set; }
    }
}



//{
//    "Countries": [{
//        "CountryCode": "DE",
//        "CountryName": "Germany",
//        "Links": [{
//            "rel": "cityPairsLookup",
//            "href": "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=DE"
//        }]
//    }, {
//        "CountryCode": "GR",
//        "CountryName": "Greece",
//        "Links": [{
//            "rel": "cityPairsLookup",
//            "href": "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=GR"
//        }]
//    }, {
//        "CountryCode": "IT",
//        "CountryName": "Italy",
//        "Links": [{
//            "rel": "cityPairsLookup",
//            "href": "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=IT"
//        }]
//    }, {
//        "CountryCode": "MX",
//        "CountryName": "Mexico",
//        "Links": [{
//            "rel": "cityPairsLookup",
//            "href": "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=MX"
//        }]
//    }, {
//        "CountryCode": "US",
//        "CountryName": "United States",
//        "Links": [{
//            "rel": "cityPairsLookup",
//            "href": "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=US"
//        }]
//    }, {
//        "CountryCode": "CA",
//        "CountryName": "Canada",
//        "Links": [{
//            "rel": "cityPairsLookup",
//            "href": "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=CA"
//        }]
//    }],
//    "Links": [{
//        "rel": "self",
//        "href": "https://api.test.sabre.com/v1/lists/supported/pointofsalecountries"
//    }, {
//        "rel": "linkTemplate",
//        "href": "https://api.test.sabre.com/v1/lists/supported/pointofsalecountries"
//    }]
//}
