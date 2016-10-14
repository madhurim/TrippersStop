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
        public static void SendMail(string firstName,string pwd,string ToEmailID) 
        {            
            string body = string.Format(@"Dear {0},<br/><br/> Thank you for registering for the {1}.<br/><br/> On verifying your email address, you will have lots of benefits while exploring your trip.
                        <br/><br/><br/> your Trippism account  password is: {2}. <br/><br/><br/> Thank you!", firstName, "Trippism", pwd);

            var result = Task.Run(() => MailgunEmail.SendComplexMessage("noreply@trippism.com", "Trippism: Registration Confirmation", new List<string>() { ToEmailID }, body));
        }
    }
}