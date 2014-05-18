namespace RedLions.Business
{
    using System;

    public class InquiryChatMessage
    {
        protected InquiryChatMessage()
        {
            // Required by EF.
        }

        public InquiryChatMessage(string senderUsername, string message)
        {
            if (string.IsNullOrEmpty(senderUsername))
            {
                throw new ArgumentNullException("senderUsername");
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            this.SentDateTime = DateTime.Now;
        }

        public int ID { get; set; }

        public virtual InquiryChatSession InquiryChatSession { get; private set; }

        public string SenderUsername { get; private set; }

        public string Message { get; private set; }

        public DateTime SentDateTime { get; private set; }
    }
}
