using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EmailService;

namespace TrippismProfiles
{
    /// <summary>
    /// This class is used for email
    /// </summary>
    public class EmailVerification
    {
        /// <summary>
        /// This method is used for sending emails
        /// </summary>

        MailgunEmail mail = new MailgunEmail();
        public void SendMail(string firstName, string pwd, string ToEmailID)
        {
            string body = string.Format(@"<br/>Thank you for registering for the Trippism.<br/><br/><br/> Your Trippism account  password is: {0}. <br/><br/>
                                                    You can change the password by clicking the change password link<br/><br/> Thank you!", pwd);

            var result = mail.SendComplexMessage("noreply@trippism.com", "Trippism: +", new List<string>() { ToEmailID }, body);
            //var result = Task.Run(() => mail.SendComplexMessage("noreply@trippism.com", "Trippism: +", new List<string>() { ToEmailID }, body));
        }

        public void SendForgotPwasswordMail(string firstName, string changePasswordUrl, string ToEmailID)
        {
            string body = string.Format(@"<br/>As per your request to reset your Trippism password.<br/><br/><a href='{0}'>Click here to change your password.</a>
                                        <br/><br/> Thank you!", changePasswordUrl);

            var result = Task.Run(() => mail.SendComplexMessage("noreply@trippism.com", "Trippism: Request for reset Password", new List<string>() { ToEmailID }, body));
        }

        public void SendUserMail(string from, string to, string subject, string htmlbody)
        {
            var result = Task.Run(() => mail.SendComplexMessage(from, subject, new List<string>() { to }, htmlbody));
        }
    }
}