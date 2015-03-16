using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraveLayer.CustomTypes.Sabre
{
    
    public class OTA_AlternateDate
    {
        public OTAAirLowFareSearchRQ OTA_AirLowFareSearchRQ { get; set; }
    }

    public class AlternateDate
    {
        public OTA_AlternateDate OTA_AlternateDate { get; set; }
    }


}



//{
//    "OTA_AirLowFareSearchRQ": {
//        "OriginDestinationInformation": [
//            {
//                "DepartureDateTime": "2014-11-11T00:00:00",
//                "DestinationLocation": {
//                    "LocationCode": "LAX"
//                },
//                "OriginLocation": {
//                    "LocationCode": "DFW"
//                },
//                "RPH": 1
//            },
//            {
//                "DepartureDateTime": "2014-11-12T00:00:00",
//                "DestinationLocation": {
//                    "LocationCode": "DFW"
//                },
//                "OriginLocation": {
//                    "LocationCode": "LAX"
//                },
//                "RPH": 2
//            }
//        ],
//        "POS": {
//            "Source": [
//                {
//                    "RequestorID": {
//                        "CompanyName": {
//                            "Code": "TN"
//                        },
//                        "ID": "REQ.ID",
//                        "Type": "0.AAA.X"
//                    }
//                }
//            ]
//        },
//        "TPA_Extensions": {
//            "IntelliSellTransaction": {
//                "RequestType": {
//                    "Name": "AD1"
//                }
//            }
//        },
//        "TravelerInfoSummary": {
//            "AirTravelerAvail": [
//                {
//                    "PassengerTypeQuantity": [
//                        {
//                            "Code": "ADT",
//                            "Quantity": 1
//                        }
//                    ]
//                }
//            ]
//        }
//    }
//}