using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippersStop.TraveLayer;
using System.Threading.Tasks;
using Newtonsoft.Json;


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

            string body = "json body";

            String result = apiWrapper.Post("v1.8.2/shop/flights?mode=live", body).Result;

            // Assert
            Assert.IsNotNull(result);

        }
    }
}
