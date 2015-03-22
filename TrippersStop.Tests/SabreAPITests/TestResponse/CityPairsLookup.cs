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

namespace TrippersStop.Tests.SabreAPITests
{
    [TestClass]
    public class CityPairsLookup
    {
        [TestMethod]
        public void GetTest()
        {
            SabreAPICaller apiWrapper = new SabreAPICaller();

            apiWrapper.Accept = "application/json";
            apiWrapper.ContentType = "application/x-www-form-urlencoded";

            string token = apiWrapper.GetToken().Result;

            apiWrapper.Authorization = "bearer";

            string result = apiWrapper.Get("v1/lists/supported/shop/flights/origins-destinations?destinationcountry=AR").Result;

            Assert.IsNotNull(result);
        }
    }
}
