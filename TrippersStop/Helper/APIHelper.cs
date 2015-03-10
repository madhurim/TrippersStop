using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TraveLayer.CustomTypes.Sabre;
using TrippersStop.TraveLayer;
using TraveLayer.APIServices;
using ServiceStack;

namespace TrippersStop.Helper
{
    public static class APIHelper
    {
        public static string GetDataFromSabre(string url)
        {
            SabreAPICaller topDestinationAPI = new SabreAPICaller();
            topDestinationAPI.Accept = "application/json";
            topDestinationAPI.ContentType = "application/x-www-form-urlencoded";
            //TBD : Aoid call for getting token
            string token = topDestinationAPI.GetToken().Result;
            topDestinationAPI.Authorization = "bearer";
            topDestinationAPI.ContentType = "application/json";
            //TBD : URL configurable using XML
            String result = topDestinationAPI.Get(url).Result;
            return result;

        }
          
    }
}