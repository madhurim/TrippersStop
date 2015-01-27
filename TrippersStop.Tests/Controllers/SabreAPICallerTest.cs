using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippersStop.SabreAPIWrapper;


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

            //GET https://api.sabre.com/v1/historical/flights/JFK/seasonality HTTP/1.1

            // Assert
            Assert.IsNotNull(result);
           
        }
    }
}
