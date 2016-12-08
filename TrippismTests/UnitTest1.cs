using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EmailService;
using System.Collections.Generic;

namespace TrippismTests
{
    [TestClass]
    public class UnitTestEmail
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<string> emails = new List<string>();
            emails.Add("madhuri.mittal@mungaru.com");
            SESEmail mail = new SESEmail();
            mail.SendMessage("noreply@trippism.com", "Does it work", emails, "This is the body");
        }
    }
}
