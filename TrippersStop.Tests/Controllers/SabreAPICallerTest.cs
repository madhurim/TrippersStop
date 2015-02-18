using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippersStop.TraveLayer;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TraveLayer.CustomTypes.Sabre;
using System.Collections.Generic;
using Moq;

namespace TrippersStop.Tests.Controllers
{
    [TestClass]
    public class SabreAPICallerTest
    {
        [TestMethod]
        public void GetToken()
        {
            // Arrange
            SabreAPICaller apiWrapper = new SabreAPICaller();

            apiWrapper.Accept = "application/json";
            apiWrapper.ContentType = "application/x-www-form-urlencoded";            

            // Act
            string result = apiWrapper.GetToken().Result;
           
            // Assert
            Assert.IsNotNull(result);
           
        }
        [TestMethod]
        public void Get()
        {
            // Arrange
            SabreAPICaller apiWrapper = new SabreAPICaller();

            apiWrapper.Accept = "application/json";
            apiWrapper.ContentType = "application/x-www-form-urlencoded";

            // Act
            string token = apiWrapper.GetToken().Result;            

            //GET https://api.sabre.com/v1/historical/flights/JFK/seasonality HTTP/1.1

            apiWrapper.Authorization = "bearer";
            apiWrapper.ContentType = "application/json";
            String result = apiWrapper.Get("v1/historical/flights/JFK/seasonality").Result;

            // Assert
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void Post()
        {
            // Arrange
            SabreAPICaller apiWrapper = new SabreAPICaller();

            apiWrapper.Accept = "application/json";
            apiWrapper.ContentType = "application/x-www-form-urlencoded";

            // Act
            string token = apiWrapper.GetToken().Result;

            //post https://api.sabre.com/v1.8.2/shop/flights?mode=live HTTP/1.1

            apiWrapper.Authorization = "bearer";
            apiWrapper.ContentType = "application/json";

            //string body = @"{"OTA_AirLowFareSearchRQ":{"OriginDestinationInformation":[{"DepartureDateTime":"2014-11-11T00:00:00","DestinationLocation":{"LocationCode":"EWR"},"OriginLocation":{"LocationCode":"DFW"},"RPH":1},{"DepartureDateTime":"2014-11-11T00:00:00","DestinationLocation":{"LocationCode":"EWR"},"OriginLocation":{"LocationCode":"DFW"},"RPH":1}],"POS":{"Source":[{"RequestorID":{"CompanyName":{"Code":"TN"},"ID":"REQ.ID","Type":"0.AAA.X"}}]},"TPA_Extensions":{"IntelliSellTransaction":{"RequestType":{"Name":"50ITINS"}},"NumTrips":{"Number":1}},"TravelerInfoSummary":{"AirTravelerAvail":[{"PassengerTypeQuantity":[{"Code":"ADT","Quantity":1}]}]},"TravelPreferences":{"TPA_Extensions":{}}}}";

            string body = "json body";
            
            String result = apiWrapper.Post("v1.8.2/shop/flights?mode=live", body).Result;

            // Assert
            Assert.IsNotNull(result);

        }
        [TestMethod]
        public void MoqTest()
        {
            //Arrange
            Mock<IAPIAsyncCaller> mockContainer = new Mock<IAPIAsyncCaller>();
            SabreAPICaller apicaller = new Mock<SabreAPICaller>().Object;

            var mock = new Mock<IAPIAsyncCaller>();
            mock.Setup(foo => foo.Get("http://localhost/").Result );

            apicaller.Accept = "application/json";
            apicaller.ContentType = "application/x-www-form-urlencoded";

            // Act
            string result = apicaller.GetToken().Result;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void SerializationTest()
        {
            BargainFinder bf = new BargainFinder();
            bf.OTA_AirLowFareSearchRQ = new OTAAirLowFareSearchRQ();

            bf.OTA_AirLowFareSearchRQ.OriginDestinationInformation = new System.Collections.Generic.List<OriginDestinationInformation>();

            OriginDestinationInformation odinfoOrig = new OriginDestinationInformation();
            odinfoOrig.DestinationLocation = new DestinationLocation();
            odinfoOrig.DestinationLocation.LocationCode = "EWR";
            odinfoOrig.OriginLocation = new OriginLocation();
            odinfoOrig.OriginLocation.LocationCode = "DFW";
            odinfoOrig.RPH = 1;
            odinfoOrig.DepartureDateTime = "2014-11-11T00:00:00";
            bf.OTA_AirLowFareSearchRQ.OriginDestinationInformation.Add(odinfoOrig);

            OriginDestinationInformation odinfoDest = new OriginDestinationInformation();
            odinfoDest.DestinationLocation = new DestinationLocation();
            odinfoDest.DestinationLocation.LocationCode = "DFW";
            odinfoDest.OriginLocation = new OriginLocation();
            odinfoDest.OriginLocation.LocationCode = "EWR";
            odinfoDest.RPH = 1;
            odinfoDest.DepartureDateTime = "2014-11-20T00:00:00";
            bf.OTA_AirLowFareSearchRQ.OriginDestinationInformation.Add(odinfoDest);

            bf.OTA_AirLowFareSearchRQ.POS = new POS();
            bf.OTA_AirLowFareSearchRQ.POS.Source = new System.Collections.Generic.List<Source>();
            bf.OTA_AirLowFareSearchRQ.POS.Source.Add(new Source()
            {
                RequestorID = new RequestorID() { CompanyName = new CompanyName() { Code = "TN" }, ID = "REQ.ID", Type = "0.AAA.X" }

            });

            bf.OTA_AirLowFareSearchRQ.TPA_Extensions = new TPAExtensions();
            bf.OTA_AirLowFareSearchRQ.TPA_Extensions.IntelliSellTransaction = new IntelliSellTransaction();
            bf.OTA_AirLowFareSearchRQ.TPA_Extensions.IntelliSellTransaction.RequestType = new RequestType()
            {
                Name = "50ITINS"

            };
            bf.OTA_AirLowFareSearchRQ.TravelPreferences = new TravelPreferences();
            bf.OTA_AirLowFareSearchRQ.TravelPreferences.TPA_Extensions = new TPAExtensions();
            bf.OTA_AirLowFareSearchRQ.TravelPreferences.TPA_Extensions.NumTrips = new NumTrips();
            bf.OTA_AirLowFareSearchRQ.TravelPreferences.TPA_Extensions.NumTrips.Number = 1;

            bf.OTA_AirLowFareSearchRQ.TravelerInfoSummary = new TravelerInfoSummary();
            bf.OTA_AirLowFareSearchRQ.TravelerInfoSummary.AirTravelerAvail = new List<AirTravelerAvail>();
            
            AirTravelerAvail air = new AirTravelerAvail();
            air.PassengerTypeQuantity = new List<PassengerTypeQuantity>();
            PassengerTypeQuantity pq = new PassengerTypeQuantity();
            pq.Quantity = 1;
            pq.Code = "ADT";
            air.PassengerTypeQuantity.Add(pq);
            bf.OTA_AirLowFareSearchRQ.TravelerInfoSummary.AirTravelerAvail.Add(air);            

            ServiceStack.Text.JsonSerializer<BargainFinder> seri = new ServiceStack.Text.JsonSerializer<BargainFinder>();
            String bfJson = seri.SerializeToString(bf);

            // Assert
            Assert.AreEqual(bfJson,String.Empty);
        }

        [TestMethod]
        public void SerializationTestForLeadPriceCalendar()
        {
            LeadPriceCalendar lpc = new LeadPriceCalendar();
            lpc.OTA_LeadPriceCalendar = new OTA_LeadPriceCalendar();

            lpc.OTA_LeadPriceCalendar.FareInfo = new System.Collections.Generic.List<FareInfo>();

            FareInfo fareInfo_ = new FareInfo();
            fareInfo_.CurrencyCode = "USD";
            fareInfo_.DepartureDateTime = "2015-03-05";
            fareInfo_.LowestFare = "342.0";
            fareInfo_.ReturnDateTime = "2013-04-08";
            fareInfo_.LowestNonStopFare = "349.8";

            lpc.OTA_LeadPriceCalendar.Links = new List<Link>();
            Link links_ = new Link();
            links_.href = "https://api.sabre.com/v1/shop/flights?origin=ATL&destination=LAS&departuredate=2013-02-05&returndate=2013-02-08&offset=<offset>&limit=<limit>&sortby=<sortby>&order=<order>&sortby2=<sortby2>&order2=<order2>";
            links_.rel = "shop";
           
        }

        

        }

    }

    
    

/*{
	"OTA_AirLowFareSearchRQ": {
		"OriginDestinationInformation": [
			{
				"DepartureDateTime": "2014-11-11T00:00:00",
				"DestinationLocation": {
					"LocationCode": "EWR"
				},
				"OriginLocation": {
					"LocationCode": "DFW"
				},
				"RPH": 1
			},
			{
				"DepartureDateTime": "2014-11-11T00:00:00",
				"DestinationLocation": {
					"LocationCode": "EWR"
				},
				"OriginLocation": {
					"LocationCode": "DFW"
				},
				"RPH": 1
			}
		],
		"POS": {
			"Source": [
				{
					"RequestorID": {
						"CompanyName": {
							"Code": "TN"
						},
						"ID": "REQ.ID",
						"Type": "0.AAA.X"
					}
				}
			]
		},
		"TPA_Extensions": {
			"IntelliSellTransaction": {
				"RequestType": {
					"Name": "50ITINS"
				}
			},
			"NumTrips": {
				"Number": 1
			}
		},
		"TravelerInfoSummary": {
			"AirTravelerAvail": [
				{
					"PassengerTypeQuantity": [
						{
							"Code": "ADT",
							"Quantity": 1
						}
					]
				}
			]
		},
		"TravelPreferences": {
			"TPA_Extensions": {}
		}
	}
}
    /*{
    "OTA_AirLowFareSearchRQ": {
        "OriginDestinationInformation": [
            {
                "DepartureDateTime": "2014-11-11T00:00:00",
                "DestinationLocation": {
                    "LocationCode": "JFK"
                },
                "OriginLocation": {
                    "LocationCode": "LAS"
                },
                "RPH": 1
            },
            {
                "DepartureDateTime": "2014-11-12T00:00:00",
                "DestinationLocation": {
                    "LocationCode": "LAS"
                },
                "OriginLocation": {
                    "LocationCode": "JFK"
                },
                "RPH": 2
            }
        ],
        "POS": {
            "Source": [
                {
                    "RequestorID": {
                        "CompanyName": {
                            "Code": "TN"
                        },
                        "ID": "REQ.ID",
                        "Type": "0.AAA.X"
                    }
                }
            ]
        },
        "TPA_Extensions": {
            "IntelliSellTransaction": {
                "RequestType": {
                    "Name": "50ITINS"
                }
            }
        },
        "TravelPreferences": {
            "TPA_Extensions": {
                "NumTrips": {
                    "Number": 1
                }
            }
        },
        "TravelerInfoSummary": {
            "AirTravelerAvail": [
                {
                    "PassengerTypeQuantity": [
                        {
                            "Code": "ADT",
                            "Quantity": 1
                        }
                    ]
                }
            ]
        }
    }
}
*/

