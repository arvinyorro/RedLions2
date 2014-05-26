$(document).ready(function () {

	var chat = $.connection.chatHub;

	chat.client.updateChatSession = function (chatMessage) {

		console.log(chatMessage);
		
	    // Get correct chat session div.
		var chatSessionIdHidden = $("input[name=chat-session-id][value='" + chatMessage.InquiryChatSessionID + "']");

		if (chatSessionIdHidden.length == 0) {
		    console.log("Creating a new chat session div.");
		    createChatSessionDiv(chatMessage);
		    return;
		}

		var sessionDiv = $("input[name=chat-session-id][value='" + chatMessage.InquiryChatSessionID + "']").parent();

		// Update thumb message.
		sessionDiv.find("p").html(chatMessage.Message);

		// Move to top
		sessionDiv.prependTo(sessionDiv.parent());
	}

	// Member messages UI.
	$(".chat-session").click(switchChatSession);
});

function createChatSessionDiv(chatMessage) {

    var sessionDiv = $('<div/>')
            .addClass("chat-session")
            .click(switchChatSession);

    var hiddenInput = $("<input/>")
        .attr("type", "hidden")
        .attr("name", "chat-session-id")
        .attr("value", chatMessage.InquiryChatSessionID);

    var nameLabel = $("<strong/>").html(chatMessage.Name);

    var messageLabel = $("<p/>").html(chatMessage.Message);

    // Build the div.
    sessionDiv.append(hiddenInput).append(nameLabel).append(messageLabel);

    // Add fade effect to its appearance.
    sessionDiv.fadeIn("slow");

    // Slide the rest of the divs.
    $("#chat-sessions").prepend(sessionDiv).slideDown("slow");
}

function switchChatSession() {

	var chatSessionID = $(this).find("input[name=chat-session-id]").val();

	console.log("Switched to session: ", chatSessionID);

	// Update current chat session ID for controls.
	$(modelIDs.chatSessionID).val(chatSessionID);

	// Clear previous chat session.
	$("#chat-box").empty();

	// Update highlighted chat session.
	$(".chat-session.selected").removeClass("selected");
	$(this).addClass("selected");

	// Update chat box.
	var chat = $.connection.chatHub;
	chat.server.registerChat(chatSessionID);
}
