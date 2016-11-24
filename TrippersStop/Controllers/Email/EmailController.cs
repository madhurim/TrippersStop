using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.JsonResult;
using EmailService;

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
                var toemail = email.To.Split(',');
                List<string> listToaddress = new List<string>();
                foreach (var toaddress in toemail)
                    listToaddress.Add(toaddress);

                MailgunEmail mail = new MailgunEmail();
                mail.SendComplexMessage(email.From, email.subject, listToaddress, email.body);
                result.Data = new { status = "ok" };

                return result;
            }
            catch (Exception ex)
            {
                result.Data = new { status = ex.Message, };
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