using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Configuration;

namespace EmailService
{
    public class SESEmail : IEmailService
    {
        public string SendMessage(string From, string subject, List<string> useremails, string htmlBody)
        {
            string returnMessage = String.Empty;

            String FROM = From;   // Replace with your "From" address. This address must be verified.
            String TO = String.Join(",", useremails); // Replace with a "To" address. If your account is still in the
            // sandbox, this address must be verified.

            //String SUBJECT = subject;
            //String BODY = htmlBody;

            MailMessage mailmessage = new MailMessage(From, TO)
            {
                Body = htmlBody,
                Subject = subject,
                IsBodyHtml = true
            };

            // Supply your SMTP credentials below. Note that your SMTP credentials are different from your AWS credentials.
            String SMTP_USERNAME = ConfigurationManager.AppSettings["SESEmailUsername"];  // Replace with your SMTP username. 
            String SMTP_PASSWORD = ConfigurationManager.AppSettings["SESEmailPassword"];  // Replace with your SMTP password.

            // Amazon SES SMTP host name. This example uses the US West (Oregon) region.
            const String HOST = "email-smtp.us-west-2.amazonaws.com";

            // The port you will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
            // STARTTLS to encrypt the connection.
            const int PORT = 587;

            // Create an SMTP client with the specified host name and port.
            using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Create a network credential with your SMTP user name and password.
                client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Use SSL when accessing Amazon SES. The SMTP session will begin on an unencrypted connection, and then 
                // the client will issue a STARTTLS command to upgrade to an encrypted connection using SSL.
                client.EnableSsl = true;

                // Send the email. 
                try
                {
                    client.Send(mailmessage);
                    //client.Send(FROM, TO, SUBJECT, BODY);
                    Console.WriteLine("Email sent!");
                }
                catch (Exception ex)
                {
                    returnMessage = "Error message: " + ex.Message;
                }
            }

            return returnMessage;

        }
    }
}
