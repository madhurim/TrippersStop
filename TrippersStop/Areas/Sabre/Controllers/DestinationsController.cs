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
using System.Reflection;
using System.Text;
using TraveLayer.CustomTypes.Sabre.ViewModel;

namespace TrippersStop.Areas.Sabre.Controllers
{
    public class DestinationsController : ApiController
    {
        // GET api/DestinationFinder
        public HttpResponseMessage Get([FromUri]Destinations destinationsRequest)
        {
            string url = GetURL(destinationsRequest);
            return GetResponse(url);
        }
        private string GetURL(Destinations destinationsRequest)
        {
            StringBuilder url = new StringBuilder();
            url.Append("v1/shop/flights/fares?");
            Type type = destinationsRequest.GetType();
            PropertyInfo[] properties = type.GetProperties();
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            string separator = string.Empty;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
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
            return url.ToString();
        }
        private HttpResponseMessage GetResponse(string url)
        {
            string result = APIHelper.GetDataFromSabre(url);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}