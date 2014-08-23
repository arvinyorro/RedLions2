namespace RedLions.CrossCutting
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    public interface IMailClient
    {
        void Send(Mail message);
    }

    public class MailClient : IMailClient
    {
        public MailClient()
        {

        }

        public void Send(Mail message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("The message must not be null.");
            }

            if (message.ToRecipients.Count == 0)
            {
                throw new Exception("Unable to send mail, no recipients found");
            }

            using (var mailClient = new SmtpClient())
            using (MailMessage smtpMail = this.CreateSmptMail(message))
            {
                mailClient.Send(smtpMail);
            }
        }

        private MailMessage CreateSmptMail(Mail message)
        {
            var sender = new MailAddress(
                message.Sender.Address,
                message.Sender.DisplayName);

            var systemMail = new MailMessage();

            systemMail.IsBodyHtml = true;

            systemMail.From = sender;

            systemMail.Body = message.Body;
            systemMail.Subject = message.Subject;

            message.ToRecipients.ToList().ForEach(x => systemMail.To.Add(new MailAddress(x.Address, x.DisplayName)));
            message.CcRecipients.ToList().ForEach(x => systemMail.CC.Add(new MailAddress(x.Address, x.DisplayName)));
            message.BccRecipients.ToList().ForEach(x => systemMail.Bcc.Add(new MailAddress(x.Address, x.DisplayName)));
            message.Attachments.ToList().ForEach(x => systemMail.Attachments.Add((Attachment)x));

            return systemMail;
        }
    }
}
