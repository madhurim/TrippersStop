using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace TrippismProfiles
{
    /// <summary>
    /// This class is used for sending email message
    /// </summary>
    public static class MailgunEmail
    {
        /// <summary>
        /// This method is used for sending email message
        /// </summary>
        public static IRestResponse SendComplexMessage(string From, string subject, List<string> useremails, string htmlBody )
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");
            

            client.Authenticator = new HttpBasicAuthenticator("api", ConfigurationManager.AppSettings["MailGunApiKey"].ToString());
            

            RestRequest request = new RestRequest();
            request.AddParameter("domain", ConfigurationManager.AppSettings["MailGundomain"].ToString(), ParameterType.UrlSegment);

            request.Resource = "{domain}/messages";
            
            if(!string.IsNullOrEmpty(From))
                request.AddParameter("from", From);
            else
                request.AddParameter("from", ConfigurationManager.AppSettings["MailGunFromemail"].ToString());
            

            foreach (var emailaddr in useremails)
                request.AddParameter("to", emailaddr);
            
            request.AddParameter("subject", subject);
            
            request.AddParameter("html", htmlBody);
            
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}