using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Net;
using TraveLayer.CustomTypes.IATA.ViewModels;
using DataLayer;
using TrippismApi.Areas.IATA.Controllers;

namespace TrippismApi.Tests.SabreAPITests.TestResponse
{
    [TestClass]
    public class IATACodeTest
    {
        [TestMethod]
        public void GetTest()
        {


           //// IDBService apiCaller = new MongoService();
           // var controller = new IATAController(apiCaller);
           // controller.Request = new HttpRequestMessage();
           // controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            
           // // Act
            
           // var response = controller.Get();
           // Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
