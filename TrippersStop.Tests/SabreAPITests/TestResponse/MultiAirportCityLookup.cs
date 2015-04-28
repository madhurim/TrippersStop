﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TraveLayer.APIServices;
using TraveLayer.CustomTypes;
using TraveLayer.CustomTypes.Sabre;
using Trippism.TraveLayer;
using TraveLayer.CustomTypes.Sabre.Response;


namespace Trippism.Tests.SabreAPITests
{
     [TestClass]
    public class MultiAirportCityLookup
    {
        [TestMethod]

        public void GetTest()
        {
            SabreAPICaller apiWrapper = new SabreAPICaller();

            apiWrapper.Accept = "application/json";
            apiWrapper.ContentType = "application/x-www-form-urlencoded";

            string token = apiWrapper.GetToken().Result;

            apiWrapper.Authorization = "bearer";

            APIResponse result = apiWrapper.Get("v1/lists/supported/cities?country=DE").Result;

            Assert.IsNotNull(result);
        }
    }
}
