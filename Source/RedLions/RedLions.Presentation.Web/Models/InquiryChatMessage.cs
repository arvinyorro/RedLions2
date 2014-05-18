using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedLions.Presentation.Web.Models
{
    public class InquiryChatMessage
    {
        public int InquiryChatSessionID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string SentDateTime { get; set; }

        public InquiryChatMessage()
        {
        }

        public InquiryChatMessage(string username, string message, DateTime sentDateTime)
        {
            this.Name = username;
            this.Message = message;
            this.SentDateTime = sentDateTime.ToString();
        }
    }
}