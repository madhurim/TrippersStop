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
        public System.Web.Mvc.JsonResult SendEmailtoUser(Email email)
        {
            var result = new JsonResult();
            try
            {

              

                var emailMsg = new MailMessage()
                {
                    Body = email.body,
                    IsBodyHtml = true,
                    Subject = email.subject,
                };

                MailAddress fromaddress = new MailAddress(email.From);
                emailMsg.From = fromaddress;

                var toemail = email.To.Split(',');
                List<string> listToaddress = new List<string>();
                foreach (var toaddress in toemail)
                {
                    listToaddress.Add(toaddress);
                    emailMsg.To.Add(toaddress);
                }

                //using (var smtpclient = new SmtpClient())
                //{
                    
                //    smtpclient.Send(emailMsg);
                //    emailMsg.Dispose();
                //}

                APIHelper.MailgunEmail.SendComplexMessage(email.From, email.subject, listToaddress, email.body);

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

        public class Email
        {
            public string From { get; set; }
            public string To { get; set; }
            public string subject { get; set; }
            public string body { get; set; }
            
        }
      
}