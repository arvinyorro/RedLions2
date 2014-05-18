$(document).ready(function () {
	// Declare a proxy to reference the hub.
	var chat = $.connection.chatHub;

	// Create a function that the hub can call to populate the stored messages.
	chat.client.populateChatLog = function (chatMessages) {
		// Populate the chat log.
		$.each(chatMessages, function (index, chatMessage) {
		    $("#chat-box").append("<div id='chat-log'><strong id='chat-name'>" + chatMessage.Name + " :</strong><div id='chat-message'>" + chatMessage.Message + "</div></div>");
		});
	};


	// Set initial focus to message input box.
	$('#message').focus();
	// Start the connection.
	$.connection.hub.start().done(function () {
		$('#send').click(function () {
		    // Call the Send method on the hub.

			// chat.server.send($('#displayname').val(), $('#message').val());
			// Clear text box and reset focus for next comment.
			// $('#message').val('').focus();
		});

		chat.server.register(modelValues.chatSessionID);
	});

});

