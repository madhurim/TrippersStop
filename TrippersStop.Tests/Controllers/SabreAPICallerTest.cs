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

            // Act
            string result = apiWrapper.GetToken().Result;

            // Assert
            Assert.IsNotNull(result);
           
        }
    }
}
