using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trippism.TraveLayer;
using System.Threading.Tasks;
using TraveLayer.CustomTypes.Sabre;
using System.Collections.Generic;
using Moq;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes;
using Trippism.Areas.Sabre.Controllers;
using System.Net.Http;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Net;

namespace Trippism.Tests.SabreAPITests
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
            ICacheService dbService = new RedisService();
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
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

          
        }

        [TestMethod]
        public void GetTestMax()
        {
            // Arrange
            //var dController = new DestinationsController();

            // Arrange
            IAsyncSabreAPICaller apiCaller = new SabreAPICaller();
            ICacheService dbService = new RedisService();
            var controller = new DestinationsController(apiCaller, dbService);
            controller.Request = new HttpRequestMessage();
            //controller.Request.SetConfiguration(new HttpConfiguration());
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            // Act
            Destinations ds = new Destinations();
            // ds.Destination = "Bos";
            ds.Origin = "CLT";
            ds.DepartureDate = "2015-04-25T00:00:00";
            ds.ReturnDate = "2015-04-26T00:00:00";
            ds.Lengthofstay = "4";
            ds.Maxfare = "450";
            var response = controller.Get(ds);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);


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
