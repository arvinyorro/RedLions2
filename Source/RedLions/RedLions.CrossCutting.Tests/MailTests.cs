namespace RedLions.CrossCutting.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RedLions.CrossCutting;

    [TestClass]
    public class MailTests
    {
        [TestMethod]
        public void SendMail()
        {
            MailClient mailClient = new MailClient();
            MailAccount recipient = new MailAccount("Yorro, Arvin", "yorro.a@gmail.com");
            MailAccount sender = new MailAccount("Yorro, Arvin", "redlions@unoredlions.com");
            Mail mail = new Mail(sender);
            mail.Body = "Test Mail";
            mail.Subject = "This is a test mail";
            mail.ToRecipients.Add(recipient);
            mailClient.Send(mail);
        }
    }
}
