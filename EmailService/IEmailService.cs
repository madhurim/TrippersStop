using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailService
    {
        string SendMessage(string From, string subject, List<string> useremails, string htmlBody);
    }
}
