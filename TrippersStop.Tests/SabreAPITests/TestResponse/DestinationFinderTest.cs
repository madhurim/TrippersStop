using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippersStop.TraveLayer;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre;
using System.Collections.Generic;
using Moq;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes;
using TrippersStop.Areas.Sabre.Controllers;
using System.Net.Http;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace TrippersStop.Tests.SabreAPITests
{
    [TestClass]
    public class DestinationFinderTest
    {
        [TestMethod]
        public void GetTest()
        {
            // Arrange
            //var dController = new DestinationsController();

            // Arrange

            IAsyncSabreAPICaller apiCaller = new SabreAPICaller();
            IDBService dbService = new RedisManager();
            var controller = new DestinationsController(apiCaller, dbService);
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            // Act
            Destinations ds = new Destinations();
           // ds.Destination = "Bos";
            ds.Origin = "CLT";
            ds.DepartureDate = "2015-04-25T00:00:00";
            ds.ReturnDate = "2015-04-26T00:00:00";
            ds.Lengthofstay = "4";
            var response = controller.Get(ds);
  

            SabreAPICaller apiWrapper = new SabreAPICaller();

            apiWrapper.Accept = "application/json";
            apiWrapper.ContentType = "application/x-www-form-urlencoded";

            // Act
            string token = apiWrapper.GetToken().Result;

            apiWrapper.Authorization = "bearer";

            //you don't need serialization here , the Get call of Destination Finder takes parameters in Url
            //build url , call UrlData
            
            //replace here with Destination Finder Url
            String result = apiWrapper.Get("v1/historical/flights/JFK/seasonality").Result;

            // Assert
            Assert.IsNotNull(result);
        }
        private string UrlData()
        {
            //Create DesinationFinder request class
            //populate the data of the class
            //build Url query parameters from this data
            return null;
        }
    }
}
