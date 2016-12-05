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
     
        public void SendUserMail(string from, string to, string subject, string htmlbody)
        {
            IEmailService mail = new SESEmail();
            var result = Task.Run(() => mail.SendMessage(from, subject, new List<string>() { to }, htmlbody));
        }
    }
}