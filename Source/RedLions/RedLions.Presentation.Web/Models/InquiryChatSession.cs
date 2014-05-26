using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedLions.Presentation.Web.Models
{
    public class InquiryChatSession
    {
        public int ID { get; set; }
        public string InquirerName { get; set; }
        public string MemberUsername { get; set; }
        public string ThumbMessage { get; set; }
        public DateTime StartedDateTime { get; set; }
    }
}