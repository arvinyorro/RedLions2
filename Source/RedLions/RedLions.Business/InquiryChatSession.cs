namespace RedLions.Business
{
    using System;
    using System.Collections.Generic;

    public class InquiryChatSession
    {
        protected InquiryChatSession()
        {
            // Required by EF.
        }

        public InquiryChatSession(Member member, string inquirerName)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            if (string.IsNullOrEmpty(inquirerName))
            {
                throw new ArgumentNullException("inquirerName");
            }

            this.Member = member;
            this.InquirerName = inquirerName;
            this.StartedDateTime = DateTime.Now;
        }

        public int ID { get; set; }

        public string InquirerName { get; private set; }

        public DateTime StartedDateTime { get; private set; }

        public virtual Member Member { get; private set; }

        public virtual ICollection<InquiryChatMessage> ChatMessages { get; private set; }
    }
}
