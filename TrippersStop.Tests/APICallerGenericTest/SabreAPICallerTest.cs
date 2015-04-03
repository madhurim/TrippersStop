using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippismApi.TraveLayer;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre;
using System.Collections.Generic;
using Moq;
using TraveLayer.CustomTypes;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippismApi.Tests.Controllers
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
            APIResponse result = apiWrapper.Get("v1/historical/flights/JFK/seasonality").Result;

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

            APIResponse result = apiWrapper.Post("v1.8.2/shop/flights?mode=live", body).Result;

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
            mock.Setup(foo => foo.Get("http://localhost/").Result);

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
            Assert.AreEqual(bfJson, String.Empty);
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
            fareInfo_.LowestFare = 342.0;
            fareInfo_.ReturnDateTime = "2013-04-08";
            fareInfo_.LowestNonStopFare = "349.8";

            lpc.OTA_LeadPriceCalendar.Links = new List<Link>();
            Link links_ = new Link();
            links_.href = "https://api.sabre.com/v1/shop/flights?origin=ATL&destination=LAS&departuredate=2013-02-05&returndate=2013-02-08&offset=<offset>&limit=<limit>&sortby=<sortby>&order=<order>&sortby2=<sortby2>&order2=<order2>";
            links_.rel = "shop";

        }

        [TestMethod]
        public void DeSerializationTest()
        {

            SabreAPICaller bargainFinderAPI = new SabreAPICaller();

            //Parse the Json Data
            //Build the Json data in the front end in such a way that , it desrializes into OriginDestinationInformation
            BargainFinder bf = new BargainFinder();

            string req = @"{  ""OriginDestinationInformation"": 
                [
			        {
				        ""DepartureDateTime"": ""2014-11-11T00:00:00"",
				        ""DestinationLocation"": {
					        ""LocationCode"": ""EWR""
				        },
				        ""OriginLocation"": {
					        ""LocationCode"": ""DFW""
				        },
				        ""RPH"": 1
			        },
			        {
				        ""DepartureDateTime"": ""2014-11-11T00:00:00"",
				        ""DestinationLocation"": {
					        ""LocationCode"": ""EWR""
				        },
				        ""OriginLocation"": {
					        ""LocationCode"": ""DFW""
				        },
				        ""RPH"": 1
			        }
		        ] 
            }";

            bf.OTA_AirLowFareSearchRQ = new OTAAirLowFareSearchRQ();
            bf.OTA_AirLowFareSearchRQ.OriginDestinationInformation = new System.Collections.Generic.List<OriginDestinationInformation>();
            List<OriginDestinationInformation> odinfoOrig = new List<OriginDestinationInformation>();
            odinfoOrig = ServiceStackSerializer.DeSerialize<List<OriginDestinationInformation>>(req);

            // Assert
            Assert.AreEqual(odinfoOrig, String.Empty);
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

        [TestMethod]
        public void SerializationTestForAdvancedCalendar()
        {
            AdvancedCalendar adc = new AdvancedCalendar();

            adc.OTA_AdvancedCalendar = new OTA_AdvancedCalendar();

            OTAAirLowFareSearchRQ lfs = new OTAAirLowFareSearchRQ();
            lfs.OriginDestinationInformation = new System.Collections.Generic.List<OriginDestinationInformation>();

            lfs.POS = new POS();
            lfs.POS.Source = new System.Collections.Generic.List<Source>();
            lfs.POS.Source.Add(new Source()
            {
                RequestorID = new RequestorID() { CompanyName = new CompanyName() { Code = "TN" }, ID = "REQ.ID", Type = "0.AAA.X" }
            });

            adc.OTA_AdvancedCalendar.OTA_AirLowFareSearchRQ.TPA_Extensions = new TPAExtensions();
            adc.OTA_AdvancedCalendar.OTA_AirLowFareSearchRQ.TPA_Extensions.IntelliSellTransaction = new IntelliSellTransaction();
            adc.OTA_AdvancedCalendar.OTA_AirLowFareSearchRQ.TPA_Extensions.NumTrips.Number = 1;
            adc.OTA_AdvancedCalendar.OTA_AirLowFareSearchRQ.TravelerInfoSummary = new TravelerInfoSummary();
            adc.OTA_AdvancedCalendar.OTA_AirLowFareSearchRQ.TravelerInfoSummary.AirTravelerAvail = new List<AirTravelerAvail>();

            DepartureDates dpdates = new DepartureDates();
            dpdates.dayOrDaysRange = new List<DayOrDaysRange>();
            var day = new DayOrDaysRange();
            day.Day = new Day() { Date = DateTime.Now.ToShortDateString() };
            dpdates.dayOrDaysRange.Add(day);


            ServiceStack.Text.JsonSerializer<AdvancedCalendar> seri = new ServiceStack.Text.JsonSerializer<AdvancedCalendar>();
            String AdvancedCalendarJson = seri.SerializeToString(adc);

            // Assert
            Assert.AreEqual(AdvancedCalendarJson, String.Empty);

            //dpDates.dayOrDaysRange = DateTime.Now; //"2015-05-06";

        }
        [TestMethod]
        public void SerializationTestForAirportsatCitiesLookup()
        {

            AirportsAtCitiesLookup airCities = new AirportsAtCitiesLookup();
            airCities.OTA_AirportsAtCitiesLookup.Airports = new List<Airport>();
            var _airports = new Airport();
            _airports.name = "Biggin Hill";
            _airports.code = "BQH";
            airCities.OTA_AirportsAtCitiesLookup.Airports.Add(_airports);

            airCities.OTA_AirportsAtCitiesLookup.Links = new List<Link>();
            Link links_ = new Link();
            links_.href = "https://api.sabre.com/v1/lists/supported/cities/LON/airports";
            links_.rel = "linkTemplate";

            ServiceStack.Text.JsonSerializer<AirportsAtCitiesLookup> seri = new ServiceStack.Text.JsonSerializer<AirportsAtCitiesLookup>();
            String airCitiesJson = seri.SerializeToString(airCities);

            // Assert
            Assert.AreEqual(airCitiesJson, String.Empty);
        }


        [TestMethod]
        public void SerializationTestForMultiAirportCityLookUp()
        {
            MultiAirportCityLookup multiAirport = new MultiAirportCityLookup();

            multiAirport.OTA_MultiAirportCityLookup.Cities = new List<City>();
            var _multicity = new City();

            _multicity.code = "BER";
            _multicity.countryCode = "DE";
            _multicity.countryName = "Germany";
            _multicity.regionName = "Europe";
            _multicity.name = "Berlin";

            _multicity.Links = new List<Link>();
            multiAirport.OTA_MultiAirportCityLookup.Cities.Add(_multicity);

            Link links_ = new Link();

            links_.href = "https://api.sabre.com/v1/lists/supported/cities/BER/airports";

            links_.rel = "airportsInCity";



            ServiceStack.Text.JsonSerializer<MultiAirportCityLookup> seri = new ServiceStack.Text.JsonSerializer<MultiAirportCityLookup>();
            String multiAirportJson = seri.SerializeToString(multiAirport);

            // Assert
            Assert.AreEqual(multiAirportJson, String.Empty);

        }


        [TestMethod]
        public void SerializationTestForTravelThemeLookup()
        {
            TravelThemeLookup travelThemes = new TravelThemeLookup();

            travelThemes.OTA_TravelThemeLookup.Themes = new List<Theme>();

            var _travelThemes = new Theme();
            _travelThemes.Themes = "BEACH";

            Link links_ = new Link();

            links_.href = "https://api.sabre.com/v1/lists/supported/shop/themes/BEACH";

            links_.rel = "destinations";

            travelThemes.OTA_TravelThemeLookup.Themes.Add(_travelThemes);

            ServiceStack.Text.JsonSerializer<TravelThemeLookup> seri = new ServiceStack.Text.JsonSerializer<TravelThemeLookup>();
            String TravelThemeLookupJson = seri.SerializeToString(travelThemes);

            // Assert
            Assert.AreEqual(TravelThemeLookupJson, String.Empty);

        }


        [TestMethod]
        public void SerializationTestForThemeAirportLookup()
        {
            ThemeAirportLookup travelThemeAirport = new ThemeAirportLookup();

            travelThemeAirport.OTA_ThemeAirportLookup.Destinations = new List<Destination>();

            var _travelThemes = new Destination();

            _travelThemes.Destinations = "FLL";
            _travelThemes.Type = "Airport";

            travelThemeAirport.OTA_ThemeAirportLookup.Destinations.Capacity = 10;

            Link links_ = new Link();

            links_.href = "https://api.sabre.com/v1/shop/flights/fares?origin=<origin>&departuredate=<departuredate>&returndate=<returndate>&location=<location>&theme=BEACH&minfare=<minfare>&maxfare=<maxfare>&lengthofstay=<lengthofstay>&earliestdeparturedate=<earliestdeparturedate>&latestdeparturedate=<latestdeparturedate>&pointofsalecountry=<pointofsalecountry>&region=<region>";
            links_.rel = "shopTemplate";
            ServiceStack.Text.JsonSerializer<ThemeAirportLookup> seri = new ServiceStack.Text.JsonSerializer<ThemeAirportLookup>();
            String TravelThemeLookupJson = seri.SerializeToString(travelThemeAirport);

            // Assert
            Assert.AreEqual(TravelThemeLookupJson, String.Empty);

        }

        [TestMethod]
        public void SerializationTestForAlternateDate()
        {
            AlternateDate alternateDates = new AlternateDate();

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TravelPreferences = new TravelPreferences();
            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TravelPreferences.TPA_Extensions = new TPAExtensions();
            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TPA_Extensions.NumTrips = new NumTrips();
            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TPA_Extensions.NumTrips.Number = 4;

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TPA_Extensions.IntelliSellTransaction = new IntelliSellTransaction();
            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TPA_Extensions.IntelliSellTransaction.RequestType.Name = "AD1";

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.OriginDestinationInformation = new List<OriginDestinationInformation>();
            var orgdest = new OriginDestinationInformation();

            orgdest.DepartureDateTime = "2015-04-10T07:10:00";
            orgdest.DestinationLocation.LocationCode = "LAX";
            orgdest.OriginLocation.LocationCode = "DFW";
            orgdest.RPH = 1;

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.OriginDestinationInformation.Add(orgdest);

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TravelerInfoSummary.AirTravelerAvail = new List<AirTravelerAvail>();

            var _airtravelAwail = new AirTravelerAvail();
            _airtravelAwail.PassengerTypeQuantity = new List<PassengerTypeQuantity>();
            PassengerTypeQuantity pq = new PassengerTypeQuantity();
            pq.Quantity = 1;
            pq.Code = "ADT";

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.TravelerInfoSummary.AirTravelerAvail.Add(_airtravelAwail);

            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.POS = new POS();
            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.POS.Source = new System.Collections.Generic.List<Source>();
            alternateDates.OTA_AlternateDate.OTA_AirLowFareSearchRQ.POS.Source.Add(new Source()
            {
                RequestorID = new RequestorID() { CompanyName = new CompanyName() { Code = "TN" }, ID = "REQ.ID", Type = "0.AAA.X" }

            });


            ServiceStack.Text.JsonSerializer<AlternateDate> seri = new ServiceStack.Text.JsonSerializer<AlternateDate>();
            String alternateDatesJson = seri.SerializeToString(alternateDates);

            // Assert
            Assert.AreEqual(alternateDatesJson, String.Empty);

        }


        [TestMethod]
        public void SerializationTestForFareRange()
        {
            FareRange farerange = new FareRange();

            farerange.OTA_FareRange.OriginLocation = "DFW";
            farerange.OTA_FareRange.DestinationLocation = "LAS";

            farerange.OTA_FareRange.FareData = new List<FareData>();

            var _fareRange = new FareData();

            _fareRange.Count = "High";
            _fareRange.CurrencyCode = "USD";
            _fareRange.DepartureDateTime = "2015-01-20T00:00:00";
            _fareRange.MaximumFare = 1262.65;
            _fareRange.MinimumFare = 88.1;
            _fareRange.MedianFare = 331.6;
            _fareRange.ReturnDateTime = "2015-01-23T00:00:00";


            farerange.OTA_FareRange.FareData.Add(_fareRange);

            Link links_ = new Link();
            links_.rel = "shop";
            links_.href = "https://api.sabre.com/v1/forecast/flights/fares?origin=DFW&destination=LAS&departuredate=2015-01-20&returndate=2015-01-23";


            ServiceStack.Text.JsonSerializer<FareRange> seri = new ServiceStack.Text.JsonSerializer<FareRange>();
            String farerangeJson = seri.SerializeToString(farerange);

            // Assert
            Assert.AreEqual(farerangeJson, String.Empty);

        }



        [TestMethod]
        public void SerializationTestForTravelSeasonality()
        {


            TravelSeasonality travelSeasonality = new TravelSeasonality();

            travelSeasonality.OTA_TravelSeasonality.DestinationLocation = "JFK";
            travelSeasonality.OTA_TravelSeasonality.Seasonality = new List<Seasonality>();

            var _travelSeasonality = new Seasonality();

            _travelSeasonality.NumberOfObservations = "GreaterThan10000";
            _travelSeasonality.WeekEndDate = "2014-01-05T00:00:00";
            _travelSeasonality.WeekStartDate = "2013-12-30T00:00:00";
            _travelSeasonality.YearWeekNumber = 1;
            _travelSeasonality.SeasonalityIndicator = "Low";

            travelSeasonality.OTA_TravelSeasonality.Seasonality.Add(_travelSeasonality);


            Link links_ = new Link();
            links_.rel = "self";
            links_.href = "https://api.sabre.com/v1/historical/flights/JFK/seasonality";


            ServiceStack.Text.JsonSerializer<TravelSeasonality> seri = new ServiceStack.Text.JsonSerializer<TravelSeasonality>();
            String travelSeasonalityJson = seri.SerializeToString(travelSeasonality);

            // Assert
            Assert.AreEqual(travelSeasonalityJson, String.Empty);

        }


        [TestMethod]
        public void SerializationTestForPointofSaleCountryCodeLookup()
        {

            PointofSaleCountryCodeLookup PointoFSaleCountry = new PointofSaleCountryCodeLookup();

            PointoFSaleCountry.OTA_PointofSaleCountryCodeLookup.Countries = new List<Country>();

            var _PointoFSaleCountry = new Country();
            _PointoFSaleCountry.CountryCode = "DE";
            _PointoFSaleCountry.CountryName = "Germany";

            PointoFSaleCountry.OTA_PointofSaleCountryCodeLookup.Countries.Add(_PointoFSaleCountry);

            Link links_ = new Link();
            links_.rel = "cityPairsLookup";
            links_.href = "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=DE";

            ServiceStack.Text.JsonSerializer<PointofSaleCountryCodeLookup> seri = new ServiceStack.Text.JsonSerializer<PointofSaleCountryCodeLookup>();
            String PointoFSaleCountryJson = seri.SerializeToString(PointoFSaleCountry);

            // Assert
            Assert.AreEqual(PointoFSaleCountryJson, String.Empty);

        }



        //[TestMethod]
        //public void SerializationTestForTravelSeasonalityAirportsLookup()
        //{

        //    TravelSeasonalityAirportsLookup travelSeasonalityAirportLookUp = new TravelSeasonalityAirportsLookup();

        //    travelSeasonalityAirportLookUp.OTA_TravelSeasonalityAirportsLookup.DestinationLocations = new List<DestinationLocation>();

        //    var destLocation = new DestinationLocation();
        //    destLocation.LocationCode.

        //}


        [TestMethod]
        public void SerializationTestForCountriesLookup()
        {

            CountriesLookup countryLookUp = new CountriesLookup();
            countryLookUp.OTA_CountriesLookup.DestinationCountries = new List<DestinationCountry>();

            var _destCountry = new DestinationCountry();
            _destCountry.CountryCode = "AU";
            _destCountry.CountryName = "Australia";

            countryLookUp.OTA_CountriesLookup.DestinationCountries.Add(_destCountry);

            countryLookUp.OTA_CountriesLookup.OriginCountries = new List<OriginCountry>();

            var _OrgCountry = new OriginCountry();
            _OrgCountry.CountryCode = "DE";
            _OrgCountry.CountryName = "Germany";
            countryLookUp.OTA_CountriesLookup.OriginCountries.Add(_OrgCountry);

            countryLookUp.OTA_CountriesLookup.PointOfSale = "DE";

            Link links_ = new Link();
            links_.rel = "self";
            links_.href = "https://api.sabre.com/v1/lists/supported/countries?pointofsalecountry=DE";

            ServiceStack.Text.JsonSerializer<CountriesLookup> seri = new ServiceStack.Text.JsonSerializer<CountriesLookup>();
            String PointoFSaleCountryJson = seri.SerializeToString(countryLookUp);

            // Assert
            Assert.AreEqual(PointoFSaleCountryJson, String.Empty);


        }




        [TestMethod]
        public void SerializationTestForCityPairsLookup()
        {

            CityPairsLookup citypairs = new CityPairsLookup();

            citypairs.OTA_CityPairsLookup.OriginDestinationLocations = new List<OriginDestinationLocation>();

            var _cityPairs = new OriginDestinationLocation();
            _cityPairs.DestinationLocation.LocationCode = "EWR";
            _cityPairs.OriginLocation.LocationCode = "DFW";

            citypairs.OTA_CityPairsLookup.OriginDestinationLocations.Add(_cityPairs);


            Link links_ = new Link();
            links_.rel = "self";
            links_.href = "https://api.test.sabre.com/v1/lists/supported/shop/flights/origins-destinations?destinationregion=<destinationregion>&originregion=<originregion>&destinationcountry=<destinationcountry>&origincountry=<origincountry>&pointofsalecountry=<pointofsalecountry>";

            ServiceStack.Text.JsonSerializer<CityPairsLookup> seri = new ServiceStack.Text.JsonSerializer<CityPairsLookup>();
            String citypairsJson = seri.SerializeToString(citypairs);

            // Assert
            Assert.AreEqual(citypairsJson, String.Empty);


        }


    }


}

       


