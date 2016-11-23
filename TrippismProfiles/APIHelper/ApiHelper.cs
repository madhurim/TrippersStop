using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using TrippismApi.TraveLayer;
using TrippismEntities;
using TrippismProfiles.Models;
using EmailService;

namespace TrippismProfiles
{
    /// <summary>
    /// This class is used for helper methods
    /// </summary>
    public static class ApiHelper
    {

        /// <summary>
        /// This method is used for generating random password
        /// </summary>
        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        /// <summary>
        /// This method is used to check the redis availability
        /// </summary>
        public static bool IsRedisAvailable()
        {
            bool isRedisAvailable = false;
            RedisService redisService = new RedisService();
            isRedisAvailable = redisService.IsConnected();
            return isRedisAvailable;
        }
        /// <summary>
        /// This method is used to regiter entites for mapping
        /// </summary>
        public static void RegisterMappingEntities()
        {
            Mapper.Register<AuthDetails, SignUpViewModel>();
            Mapper.Register<SearchCriteria, SearchActivityViewModel>()
                // .Member(h => h.SearchType, m => m.SearchedTypeID)
                   .Member(h => h.CustomerId, m => m.RefGuid);
            Mapper.Register<MyDestinationsViewModel, MyDestinations>();
            Mapper.Compile();
        }
        /// <summary>
        /// This method is used for getting the IP address
        /// </summary>
        public static string GetClientIP(HttpRequestMessage request)
        {
            // request = request ?? Request;
            const string HttpContextName = "MS_HttpContext";
            const string RemoteEndpointMessage =
                  "System.ServiceModel.Channels.RemoteEndpointMessageProperty";
            if (request.Properties.ContainsKey(HttpContextName))
            {
                return ((HttpContextWrapper)request.Properties[HttpContextName]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic prop = request.Properties[RemoteEndpointMessage];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            return null;
        }
        /// <summary>
        /// This method is used to get user agent information
        /// </summary>
        public static string GetClientUserAgent(HttpRequestMessage request)
        {
            var userAgent = ((System.Web.HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserAgent;
            return userAgent;
        }

    }
}
