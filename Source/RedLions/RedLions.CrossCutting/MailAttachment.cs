namespace RedLions.CrossCutting
{
    using System.IO;
    using System.Net.Mail;

    public interface IMailAttachment
    {
        string Filename { get; }
        Stream ContentStream { get; }
    }

    public class MailAttachment : Attachment, IMailAttachment
    {
        public MailAttachment(string filename, Stream contentStream)
            : base(contentStream, filename)
        {
            this.Filename = filename;
        }

        public string Filename { get; private set; }
    }
}
