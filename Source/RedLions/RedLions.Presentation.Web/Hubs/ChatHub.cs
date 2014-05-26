using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RedLions.Application;
using DTO = RedLions.Application.DTO;
using RedLions.Presentation.Web.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using AutoMapper;

namespace RedLions.Presentation.Web.Hubs
{
    public class ChatHub : Hub
    {
        /// <summary>
        /// The list of users connected.
        /// </summary>
        public static List<string> ConnectedUsers = new List<string>(); 

        private InquiryChatService inquiryChatService;
        private MemberService memberService;

        public ChatHub(
            InquiryChatService inquiryChatService,
            MemberService memberService)
        {
            this.inquiryChatService = inquiryChatService;
            this.memberService = memberService;
        }

        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.

            string username = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(username))
            {
                ChatHub.ConnectedUsers.Add(username);
                Clients.Group(username).MemberConnected(username);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.

            string username = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                ChatHub.ConnectedUsers.Remove(username);
                Clients.Group(username).MemberDisconnected(username);
            }

            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.

            string username = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(username))
            {
                ChatHub.ConnectedUsers.Add(username);
                Clients.Group(username).MemberConnected(username);
            }

            return base.OnReconnected();
        }

        public void RegisterChat(int chatSessionID)
        {
            // If not exists, create new group using unique inquiry ID (single user group)
            string groupName = chatSessionID.ToString();
            base.Groups.Add(Context.ConnectionId, groupName);

            // Retrieve chat session using chat session ID
            DTO.InquiryChatSession chatSessionDTO = this.inquiryChatService.GetSessionByID(chatSessionID);
            if (chatSessionDTO == null)
            {
                return;
            }
            
            // Retrieve saved chat messages from existing chat session.
            IEnumerable<DTO.InquiryChatMessage> chatMessageDTOList = this.inquiryChatService
                .GetChatMessagesBySession(chatSessionDTO.ID);
            IEnumerable<Models.InquiryChatMessage> chatMessages = Mapper.Map<IEnumerable<Models.InquiryChatMessage>>(chatMessageDTOList);

            // Send saved messages to user.
            Clients.Client(Context.ConnectionId).populateChatLog(chatMessages);
        }

        public void RegisterMember(string username)
        {
            Groups.Add(Context.ConnectionId, username);
        }

        public void RegisterInquirer()
        {
            // Retrieve referrer in cookie.
            Cookie cookie = Context.Request.Cookies["ReferrerUsername"];
            bool referrerNotInCookies = (Context.Request.Cookies["ReferrerUsername"] == null);
            if (referrerNotInCookies)
            {
                throw new Exception("Referrer not found.");
            }
            string referrerUsername = cookie.Value;

            Groups.Add(Context.ConnectionId, referrerUsername);
        }

        public bool ReferrerIsOnline()
        {
            // Retrieve referrer in cookie.
            Cookie cookie = Context.Request.Cookies["ReferrerUsername"];
            bool referrerNotInCookies = (Context.Request.Cookies["ReferrerUsername"] == null);
            if (referrerNotInCookies)
            {
                throw new Exception("Referrer not found.");
            }
            string referrerUsername = cookie.Value;

            return ChatHub.ConnectedUsers.Contains(referrerUsername);
        }

        public void Send(string data)
        {
            // Bind to model.
            InquiryChatMessage inquiryChatMessage = JsonConvert.DeserializeObject<InquiryChatMessage>(data);

            // Save message to database.
            DTO.InquiryChatMessage chatMessageDTO = Mapper.Map<DTO.InquiryChatMessage>(inquiryChatMessage);
            this.inquiryChatService.SaveMessage(chatMessageDTO);

            // Broadcast message to sender and recipient.
            int inquiryChatSessionID = inquiryChatMessage.InquiryChatSessionID;
            Clients.Group(inquiryChatSessionID.ToString()).BroadcastMessage(inquiryChatMessage);

            // Update the member chat sessions panel.
            DTO.InquiryChatSession chatSessionDTO = this.inquiryChatService.GetSessionByID(inquiryChatSessionID);
            Clients.Group(chatSessionDTO.MemberUsername).UpdateChatSession(inquiryChatMessage);


            // TODO:
            // Notify member
        }
    }
}