using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using TraveLayer.APIServices;
using ServiceStack;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class BargainFinderOldController : ApiController
    {
        // GET api/bargainfinder
        public IEnumerable<string> Get()
        {          
            
            return new string[] { "value1", "value2" };
        }

        // GET api/bargainfinder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/bargainfinder
        public HttpResponseMessage Post([FromBody]string value)
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
            OriginDestinationInformation odinfoOrig = new OriginDestinationInformation();
          //  odinfoOrig = ServiceStackSerializer.DeSerialize(req).ConvertTo<OriginDestinationInformation>();
            String result = bargainFinderAPI.Post("v1.8.2/shop/flights?mode=live", PopulateBargainFinder(bf)).Result;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            

            return response;
        }

        // PUT api/bargainfinder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/bargainfinder/5
        public void Delete(int id)
        {
        }
        private string PopulateBargainFinder(BargainFinder bf)
        {
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

            String bfJson =ServiceStackSerializer.Serialize(bf);

            //ServiceStack.Text.JsonSerializer<BargainFinder> seri = new ServiceStack.Text.JsonSerializer<BargainFinder>();
            //String bfJson = seri.SerializeToString(bf);

            return bfJson;

        }
    }
}
