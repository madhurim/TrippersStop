using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using System.Collections.Generic;
using TraveLayer.CustomTypes.Sabre.Response;

namespace TrippersStop.Tests.SabreAPITests
{
    [TestClass]
    public class BargainFinderTest
    {
        [TestMethod]
        public void PostTest()
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

            string body = SerializeData();

            APIResponse result = apiWrapper.Post("v1.8.2/shop/flights?mode=live", body).Result;

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
        private string SerializeData()
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
            odinfoOrig.DepartureDateTime = "2015-11-11T00:00:00";
            bf.OTA_AirLowFareSearchRQ.OriginDestinationInformation.Add(odinfoOrig);

            OriginDestinationInformation odinfoDest = new OriginDestinationInformation();
            odinfoDest.DestinationLocation = new DestinationLocation();
            odinfoDest.DestinationLocation.LocationCode = "DFW";
            odinfoDest.OriginLocation = new OriginLocation();
            odinfoDest.OriginLocation.LocationCode = "EWR";
            odinfoDest.RPH = 1;
            odinfoDest.DepartureDateTime = "2015-11-20T00:00:00";
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
            return bfJson;

        }
    }
}
