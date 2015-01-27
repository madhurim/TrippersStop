using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippersStop.SabreAPIWrapper;
using System.Threading.Tasks;


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
    }
}
