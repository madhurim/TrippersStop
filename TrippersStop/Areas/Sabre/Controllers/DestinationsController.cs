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
using TrippersStop.Helper;
using TrippersStop.Areas.Sabre.Models;
using System.Reflection;
using System.Text;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class DestinationsController : ApiController
    {
        // GET api/DestinationFinder
        public HttpResponseMessage Get([FromUri]DestinationsRequest destinationsRequest)
        {
            StringBuilder url = new StringBuilder ();
            url.Append("v1/shop/flights/fares?");
            Type type = destinationsRequest.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name ;               
                var result = property.GetValue(destinationsRequest, null);
                if (result != null)
                {
                    propertyValue = result.ToString();
                    if (!string.IsNullOrWhiteSpace(propertyValue))
                    {
                        url.Append(separator);
                        url.Append(string.Join("=", propertyName, propertyValue));
                        separator = "&";
                    }
                }
                 
            }
            return GetResponse(url.ToString());
        }
        // GET api/DestinationFinder
        //public HttpResponseMessage Get(string origin, string departuredate, string returndate)
        //{
        //    string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}", origin, departuredate, returndate);
        //    return GetResponse(url);
        //}
        //// GET api/DestinationFinder
        //public HttpResponseMessage Get(string origin, string lengthofstay)
        //{
        //    string url = string.Format("v1/shop/flights/fares?origin={0}&lengthofstay={1}", origin, lengthofstay);
        //    return GetResponse(url);
        //}
        //public HttpResponseMessage Get(string origin, string earliestdeparturedate, string latestdeparturedate, string lengthofstay)
        //{
        //    string url = string.Format("v1/shop/flights/fares?origin={0}&earliestdeparturedate={1}&latestdeparturedate={2}&lengthofstay={3}", origin, earliestdeparturedate, latestdeparturedate, lengthofstay);
        //    return GetResponse(url);
        //}
        //public HttpResponseMessage Get(string origin, string earliestdeparturedate, string latestdeparturedate, string lengthofstay, string theme)
        //{
        //    //string url = string.Format("v1/shop/flights/fares?origin={0}&earliestdeparturedate={1}&latestdeparturedate={2}&lengthofstay={3}&theme={4}", destinationFinderRequest.Origin, destinationFinderRequest.Earliestdeparturedate, destinationFinderRequest.Latestdeparturedate, destinationFinderRequest.Lengthofstay, destinationFinderRequest.Theme);
        //    string url = string.Format("v1/shop/flights/fares?origin={0}&earliestdeparturedate={1}&latestdeparturedate={2}&lengthofstay={3}&theme={4}", origin, earliestdeparturedate, latestdeparturedate, lengthofstay, theme);
        //    return GetResponse(url);
        //}
        //public HttpResponseMessage GetDestinationByMaxFare(string origin, string departuredate, string returndate, string maxfare)
        //{
        //    string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&maxfare={3}", origin, departuredate, returndate, maxfare);
        //    return GetResponse(url);
        //}
        //public HttpResponseMessage GetTopDestination(string origin, string departuredate, string returndate, string topdestinations)
        //{
        //    string url = string.Format("v1/shop/flights/fares?origin={0}&departuredate={1}&returndate={2}&topdestinations={3}", origin, departuredate, returndate, topdestinations);
        //    return GetResponse(url);
        //}
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}