using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrippersStop.TraveLayer;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TraveLayer.CustomTypes.Sabre;
using System.Collections.Generic;
using Moq;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes;

namespace TrippersStop.Tests.SabreAPITests
{
    [TestClass]
    public class DestinationFinderTest
    {
        [TestMethod]
        public void GetTest()
        {
            // Arrange
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
