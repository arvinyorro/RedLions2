using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedLions.Presentation.Web.ViewModels
{
    using RedLions.Presentation.Web.Models;

    public class MemberMessagesViewModel
    {
        public MemberMessagesViewModel()
        {
            this.ChatSessions = new List<InquiryChatSession>();
        }

        public IEnumerable<InquiryChatSession> ChatSessions { get; set; }
        public int SelectedChatSessionID { get; set; }
        public InquiryChatMessage ChatMessage { get; set; }
    }
}