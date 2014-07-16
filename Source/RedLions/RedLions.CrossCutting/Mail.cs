namespace RedLions.CrossCutting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Mail
    {
        public Mail(IMailAccount sender)
        {
            if (sender == null)
            {
                throw new ArgumentNullException("The sender must not be null.");
            }

            this.Sender = sender;
            this.ToRecipients = new List<IMailAccount>();
            this.CcRecipients = new List<IMailAccount>();
            this.BccRecipients = new List<IMailAccount>();
            this.Attachments = new List<IMailAttachment>();
        }

        public IMailAccount Sender { get; private set; }

        public ICollection<IMailAccount> ToRecipients { get; set; }

        public ICollection<IMailAccount> CcRecipients { get; set; }

        public ICollection<IMailAccount> BccRecipients { get; set; }

        public ICollection<IMailAttachment> Attachments { get; set; }

        public string Body { get; set; }

        public string Subject { get; set; }
    }
}
