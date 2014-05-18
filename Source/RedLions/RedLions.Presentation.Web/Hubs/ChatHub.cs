using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using RedLions.Presentation.Web.Models;
using Microsoft.AspNet.SignalR;

namespace RedLions.Presentation.Web.Hubs
{
    public class ChatHub : Hub
    {
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

            // Add client connection ID to group

            // Retrieve chat session using chat session ID

            // If none found, throw error.

            // Retrieve chat messages from chat session.

            var chatMessages = new List<InquiryChatMessage>();
            chatMessages.Add(new InquiryChatMessage(
                username: "test_member1",
                message: "Hi",
                sentDateTime: DateTime.Now));

            chatMessages.Add(new InquiryChatMessage(
                username: "user12314",
                message: "Koya",
                sentDateTime: DateTime.Now));

            // TODO:
            // Send to group self (single user group)
            Clients.All.populateChatLog(chatMessages);

            // TODO:
            // Notify member
        }
    }
}