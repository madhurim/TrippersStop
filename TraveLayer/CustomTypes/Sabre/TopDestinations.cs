using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    //class TopDestinations
    //{
    //}
    //public class Link
    //{
    //    public string rel { get; set; }
    //    public string href { get; set; }
    //}

    public class Destination2
    {
        public string DestinationLocation { get; set; }
        public string AirportName { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string Type { get; set; }
        public string MetropolitanAreaName { get; set; }
        public List<Link> Links { get; set; }
    }

    //public class Destination
    //{
    //    public int Rank { get; set; }
    //    public Destination2 Destination { get; set; }
    //}

    public class Link2
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class OTA_TopDestinations
    {
        public string OriginLocation { get; set; }
        public List<Destination> Destinations { get; set; }
        public List<Link2> Links { get; set; }
    }


    public class TopDestinations : ICustomType
    {
        public OTA_TopDestinations OTA_TopDestinations { get; set; }
    }

}



//{
//    "OriginLocation": "DFW",
//    "Destinations": [
//        {
//            "Rank": 1,
//            "Destination": {
//                "DestinationLocation": "CUN",
//                "AirportName": "Cancun",
//                "CityName": "Cancun",
//                "CountryCode": "MX",
//                "CountryName": "Mexico",
//                "RegionName": "Latin America",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 2,
//            "Destination": {
//                "DestinationLocation": "BOM",
//                "AirportName": "Chhatrapati Shivaji",
//                "CityName": "Mumbai",
//                "CountryCode": "IN",
//                "CountryName": "India",
//                "RegionName": "Asia Pacific",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 3,
//            "Destination": {
//                "DestinationLocation": "MBJ",
//                "AirportName": "Sangster Intl",
//                "CityName": "Montego Bay",
//                "CountryCode": "JM",
//                "CountryName": "Jamaica",
//                "RegionName": "North America",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 4,
//            "Destination": {
//                "DestinationLocation": "SJU",
//                "AirportName": "Luis Muoz Marn International Airport",
//                "CityName": "San Juan",
//                "CountryCode": "PR",
//                "CountryName": "Puerto Rico",
//                "RegionName": "North America",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 5,
//            "Destination": {
//                "DestinationLocation": "MAA",
//                "AirportName": "Chennai",
//                "CityName": "Chennai",
//                "CountryCode": "IN",
//                "CountryName": "India",
//                "RegionName": "Asia Pacific",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 6,
//            "Destination": {
//                "DestinationLocation": "SJD",
//                "AirportName": "Los Cabos",
//                "CityName": "San Jose Cabo",
//                "CountryCode": "MX",
//                "CountryName": "Mexico",
//                "RegionName": "Latin America",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 7,
//            "Destination": {
//                "DestinationLocation": "CHI",
//                "CountryCode": "US",
//                "CountryName": "United States",
//                "RegionName": "North America",
//                "MetropolitanAreaName": "Chicago",
//                "Links": [
//                    {
//                        "rel": "airportsInCity",
//                        "href": "https://api.sabre.com/v1/lists/supported/cities/CHI/airports"
//                    }
//                ],
//                "Type": "City"
//            }
//        },
//        {
//            "Rank": 49,
//            "Destination": {
//                "DestinationLocation": "FLL",
//                "AirportName": "Fort Lauderdale - Hollywood Internationa",
//                "CityName": "Fort Lauderdale",
//                "CountryCode": "US",
//                "CountryName": "United States",
//                "RegionName": "North America",
//                "Type": "Airport"
//            }
//        },
//        {
//            "Rank": 50,
//            "Destination": {
//                "DestinationLocation": "MIA",
//                "AirportName": "Miami International Airport",
//                "CityName": "Miami",
//                "CountryCode": "US",
//                "CountryName": "United States",
//                "RegionName": "North America",
//                "Type": "Airport"
//            }
//        }
//    ],
//    "Links": [
//        {
//            "rel": "self",
//            "href": "https://api.sabre.com/v1/lists/top/destinations?origin=dfw"
//        },
//        {
//            "rel": "linkTemplate",
//            "href": "https://api.sabre.com/v1/lists/top/destinations?origin=<origin>&origincountry=<origincountry>&topdestinations=<topdestinations>&destinationtype=<destinationtype>&theme=<theme>&destinationcountry=<destinationcountry>&region=<region>"
//        }
//    ]
//}
