using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using TrippersStop.Areas.Sabre.Controllers;
using System.Net.Http;
using System.Web.Http.Hosting;
using TraveLayer.CustomTypes.Sabre.ViewModels;
using System.Web.Http;
using System.Net;

namespace TrippersStop.Tests.SabreAPITests
{
    [TestClass]
    public class LowFareForecast
    {
        [TestMethod]

        public void GetTest()
        {
            // Arrange
            //var dController = new DestinationsController();

            // Arrange
            var controller = new LowFareForecastController();
            controller.Request = new HttpRequestMessage();
            //controller.Request.SetConfiguration(new HttpConfiguration());
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            // Act
            TravelInfo ds = new TravelInfo();
            // ds.Destination = "Bos";
            ds.Origin = "ATL";
            ds.DepartureDate = "2015-04-25T00:00:00";
            ds.ReturnDate= "2015-04-26T00:00:00";
            ds.Destination= "LAS";
            //ds.Lengthofstay = "4";
            var response = controller.Get(ds);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
