$(document).ready(function () {
	
    // Declare a proxy to reference the hub.
    var chat = $.connection.chatHub;


	// Create a function that the hub can call to populate the stored messages.
	chat.client.populateChatLog = function (chatMessages) {
		// Populate the chat log.
		$.each(chatMessages, function (index, chatMessage) {
		    appendMessage(chatMessage);
		});
	};

    // Receive Message.
	chat.client.broadcastMessage = function (chatMessage) {
	    appendMessage(chatMessage);
	}

	// Set initial focus to message input box.
	$(modelIDs.Message).focus();

	// Start the connection.
	$.connection.hub.start().done(function () {

	    $(modelIDs.sendButton).click(function () {

		    // Call the Send method on the hub.
		    var inquiryChatMessage = new Object();
		    inquiryChatMessage.InquiryChatSessionID = $(modelIDs.chatSessionID).val();
		    inquiryChatMessage.Message = $(modelIDs.message).val();
		    inquiryChatMessage.Name = $(modelIDs.name).val();
		    var serializedData = JSON.stringify(inquiryChatMessage);

		    console.log(serializedData);
		    chat.server.send(serializedData);

			// Clear text box and reset focus for next comment.
		    $(modelIDs.message).val('').focus();
		});

	    var chatSessionID = $(modelIDs.chatSessionID).val();
	    chat.server.registerChat(chatSessionID);

        // This condition is short circuited.
	    if (typeof modelValues !== 'undefined' &&
            typeof modelValues.username !== 'undefined')
	    {
	        console.log("Attempt to register member using username: ", modelValues.username);
	        chat.server.registerMember(modelValues.username);
	    }
	});
});

function appendMessage(chatMessage) {
    console.log("Message recieved: ", chatMessage);
    $("#chat-box").append("<div id='chat-log'><strong id='chat-name'>" + chatMessage.Name + " :</strong><div id='chat-message'>" + chatMessage.Message + "</div></div>");
    // $("#chat-box").animate({ scrollTop: $("#chat-box").height() }, "slow");
    $('#chat-box').scrollTop($('#chat-box').prop("scrollHeight"));
}

