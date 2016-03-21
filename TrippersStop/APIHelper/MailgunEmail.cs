using System;
using System.Collections.Generic;
using System.Configuration;
using RestSharp;

namespace Trippism.APIHelper
{
    public static class MailgunEmail
    {
        public static IRestResponse SendComplexMessage(string From, string subject, List<string> useremails, string htmlBody )
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v2");
            

            //client.Authenticator = new HttpBasicAuthenticator("api","key-af8a0a66e356d81a17e67c24f8afb43c");
            client.Authenticator = new HttpBasicAuthenticator("api", ConfigurationManager.AppSettings["MailGunApiKey"].ToString());
            

            RestRequest request = new RestRequest();
            //request.AddParameter("domain","sandboxd21b1c47661845f288d674d57918a443.mailgun.org", ParameterType.UrlSegment);
            request.AddParameter("domain", ConfigurationManager.AppSettings["MailGundomain"].ToString(), ParameterType.UrlSegment);

            request.Resource = "{domain}/messages";
            //request.AddParameter("from", "Excited User <shaileshsakaria@gmail.com>");
            
            if(!string.IsNullOrEmpty(From))
                request.AddParameter("from", From);
            else
                request.AddParameter("from", ConfigurationManager.AppSettings["MailGunFromemail"].ToString());
            

            foreach (var emailaddr in useremails)
                request.AddParameter("to", emailaddr);
            
            //request.AddParameter("cc", "baz@example.com");
            //request.AddParameter("bcc", "bar@example.com");
            request.AddParameter("subject", subject);
            
            //request.AddParameter("html", "<html>HTML version of the body</html>");
            request.AddParameter("html", htmlBody);
            
            request.Method = Method.POST;
            return client.Execute(request);
        }
    }
}