using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private InquiryChatService inquiryChatService;

        public ChatHub(InquiryChatService inquiryChatService)
        {
            this.inquiryChatService = inquiryChatService;
        }

        public override Task OnConnected()
        {
            // Add your own code here.
            // For example: in a chat application, record the association between
            // the current connection ID and user name, and mark the user as online.
            // After the code in this method completes, the client is informed that
            // the connection is established; for example, in a JavaScript client,
            // the start().done callback is executed.
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            // Add your own code here.
            // For example: in a chat application, mark the user as offline, 
            // delete the association between the current connection id and user name.
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            // Add your own code here.
            // For example: in a chat application, you might have marked the
            // user as offline after a period of inactivity; in that case 
            // mark the user as online again.
            return base.OnReconnected();
        }

        public void Register(int chatSessionID)
        {
            // If not exists, create new group using unique inquiry ID (single user group)
            string groupName = chatSessionID.ToString();
            base.Groups.Add(Context.ConnectionId, groupName);

            // Retrieve chat session using chat session ID
            DTO.InquiryChatSession chatSessionDTO = this.inquiryChatService.GetSessionByID(chatSessionID);
            if (chatSessionDTO == null)
            {
                throw new Exception("Chat session not found");
            }
            
            // Retrieve chat messages from chat session.
            IEnumerable<DTO.InquiryChatMessage> chatMessageDTOList = this.inquiryChatService
                .GetChatMessagesBySession(chatSessionDTO.ID);
            IEnumerable<Models.InquiryChatMessage> chatMessages = Mapper.Map<IEnumerable<Models.InquiryChatMessage>>(chatMessageDTOList);

            // Send to group self (single user group)
            Clients.Client(Context.ConnectionId).populateChatLog(chatMessages);

            // TODO:
            // Notify member
        }

        public void Send(string data)
        {
            InquiryChatMessage inquiryChatMessage = JsonConvert.DeserializeObject<InquiryChatMessage>(data);
            DTO.InquiryChatMessage chatMessageDTO = Mapper.Map<DTO.InquiryChatMessage>(inquiryChatMessage);

            this.inquiryChatService.SaveMessage(chatMessageDTO);

            string groupName = inquiryChatMessage.InquiryChatSessionID.ToString();
            Clients.Group(groupName).BroadcastMessage(inquiryChatMessage);
        }
    }
}