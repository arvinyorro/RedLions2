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

	chat.client.broadcastMessage = function (chatMessage) {
	    appendMessage(chatMessage);
	}

	chat.client.updateChatSessions = function (chatMessage) {
	    // 
	}

	// Set initial focus to message input box.
	$(modelIDs.Message).focus();
	// Start the connection.
	$.connection.hub.start().done(function () {

	    var chatSessionID = $(modelIDs.chatSessionID).val();

	    $(modelIDs.sendButton).click(function () {

		    // Call the Send method on the hub.
		    var inquiryChatMessage = new Object();
		    inquiryChatMessage.InquiryChatSessionID = chatSessionID;
		    inquiryChatMessage.Message = $(modelIDs.message).val();
		    inquiryChatMessage.Name = $(modelIDs.name).val();
		    var serializedData = JSON.stringify(inquiryChatMessage);

		    console.log(serializedData);
		    chat.server.send(serializedData);

			// Clear text box and reset focus for next comment.
		    $(modelIDs.message).val('').focus();
		});

	    chat.server.register(chatSessionID);
	});

});

function appendMessage(chatMessage) {
    $("#chat-box").append("<div id='chat-log'><strong id='chat-name'>" + chatMessage.Name + " :</strong><div id='chat-message'>" + chatMessage.Message + "</div></div>");
    // $("#chat-box").animate({ scrollTop: $("#chat-box").height() }, "slow");
    $('#chat-box').scrollTop($('#chat-box').prop("scrollHeight"));
}

function switchChatSession() {

    // TODO:
    // Get new chat session ID

    var chatSessionID = 1;

    $(modelIDs.chatSessionID).val(chatSessionID);
    chat.server.register(chatSessionID);
}

