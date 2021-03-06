﻿/// <summary>
/// This class retrieves up to 200 roundtrip flight itineraries with published fares available from the Sabre 
///  cache for a specific origin, destination, and length of stay across a large set or range of travel dates. 
///  The returns and sorts these itineraries by fare in ascending order, from lowest to highest.
/// </summary>





using System;
using System.Collections.Generic;


namespace TraveLayer.CustomTypes.Sabre
{   
        public class Day
        {
            public string Date { get; set; }
        }

        public class DayOrDaysRange
        {
            public Day Day { get; set; }
        }

        public class DepartureDates
        {
            public List<DayOrDaysRange> dayOrDaysRange { get; set; }
        }

         
        public class OTAAirLowFareSearchRQ
        {
            public List<OriginDestinationInformation> OriginDestinationInformation { get; set; }
            public POS POS { get; set; }
            public TPAExtensions TPA_Extensions { get; set; }
            public TravelerInfoSummary TravelerInfoSummary { get; set; }
            public TravelPreferences TravelPreferences { get; set; } // copying from AlternateDates.
        }

        public class OTA_AdvancedCalendar
        {
            public OTAAirLowFareSearchRQ OTA_AirLowFareSearchRQ { get; set; }
        }

        public class AdvancedCalendar
        {
            public OTA_AdvancedCalendar OTA_AdvancedCalendar { get; set; }
        }
    }



//{
//    "OTA_AirLowFareSearchRQ": {
//        "OriginDestinationInformation": [
//            {
//                "DepartureDates": {
//                    "dayOrDaysRange": [
//                        {
//                            "Day": {
//                                "Date": "2015-05-05"
//                            }
//                        }
//                    ]
//                },
//                "DestinationLocation": {
//                    "LocationCode": "LAX"
//                },
//                "OriginLocation": {
//                    "LocationCode": "DFW"
//                },
//                "RPH": 1
//            },
//            {
//                "DepartureDates": {
//                    "dayOrDaysRange": [
//                        {
//                            "Day": {
//                                "Date": "2015-05-06"
//                            }
//                        }
//                    ]
//                },
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
//                    "Name": "ADC1000"
//                }
//            }
//        },
//        "TravelPreferences": {
//            "TPA_Extensions": {
//                "NumTrips": {
//                    "Number": 1
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


