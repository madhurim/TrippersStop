using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trippism.Areas.IATA.Controllers;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Net;

namespace Trippism.Tests.SabreAPITests.TestResponse
{
    [TestClass]
    public class IATACodeTest
    {
        [TestMethod]
        public void GetTest()
        {
            var controller = new IATAController();
            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            
            // Act
            
            var response = controller.Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
