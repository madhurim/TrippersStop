using System;
using System.Collections.Generic;
using System.Linq; 
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceStack;
using System.Reflection;
using System.Text;
using AutoMapper;
using System.Configuration;
using System.Web.Http.Description;
using System.Net.Mail;
using System.Runtime.Serialization;
using Newtonsoft.JsonResult;


namespace Trippism.Controllers.Email
{
    public class EmailController : ApiController
    {
        
        [HttpPost]
        public System.Web.Mvc.JsonResult SendEmailtoUser(EmailDet emaildet)
        {
            var result = new JsonResult();
            try
            {
                var email = new MailMessage()
                {
                    Body = emaildet.body,
                    IsBodyHtml = true,
                    Subject = emaildet.subject,
                };

                MailAddress fromaddress = new MailAddress(emaildet.From);
                email.From = fromaddress;

                var toemail = emaildet.To.Split(',');

                foreach (var toaddress in toemail)
                {
                    email.To.Add(toaddress);
                }

                using (var smtpclient = new SmtpClient())
                {
                    
                    smtpclient.Send(email);
                    email.Dispose();
                }              

                result.Data = new
                {
                    status = "ok",
                };

                return result;
            }
            catch(Exception ex)
            {
                result.Data = new
                {
                    status = ex.Message,
                };

                return result;
            }
        }

    }

        public class EmailDet
        {
            public string From { get; set; }
            public string To { get; set; }
            public string subject { get; set; }
            public string body { get; set; }
            public string mapImage { get; set; }
        }
      
}