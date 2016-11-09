using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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
        public static void SendMail(string firstName, string pwd, string ToEmailID)
        {
            string body = string.Format(@"<br/>Thank you for registering for the Trippism.<br/><br/><br/> Your Trippism account  password is: {0}. <br/><br/>
                                            You can change the password by clicking the change password link<br/><br/> Thank you!", pwd);

            var result = Task.Run(() => MailgunEmail.SendComplexMessage("noreply@trippism.com", "Trippism: Registration Confirmation", new List<string>() { ToEmailID }, body));
        }
        public static void SendForgotPwasswordMail(string firstName, string changePasswordUrl, string ToEmailID)
        {
            string body = string.Format(@"<br/>As per your request to reset your Trippism password.<br/><br/><a href='{0}'>Click here to change your password.</a>
                                        <br/><br/> Thank you!", changePasswordUrl);

            var result = Task.Run(() => MailgunEmail.SendComplexMessage("noreply@trippism.com", "Trippism: Request for reset Password", new List<string>() { ToEmailID }, body));
        }
    }
}