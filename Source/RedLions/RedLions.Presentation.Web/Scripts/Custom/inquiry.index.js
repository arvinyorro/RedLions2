$(document).ready(function () {

    // Declare a proxy to reference the hub.
    var chat = $.connection.chatHub;

    // Receive Message.
    chat.client.memberConnected = function (username) {
        console.log("Referrer is online: ", username);
        enableChatButton();
    }

    chat.client.memberDisconnected = function (username) {
        console.log("Referrer is offline: ", username);
        disableChatButton();
    }

    // Start the connection.
    $.connection.hub.start().done(function () {

        

        chat.server.registerInquirer();
        chat.server.referrerIsOnline().done(function (isOnline) {
            if (isOnline) {
                enableChatButton()
            }
            else {
                disableChatButton();
            }
        });

        console.log("Connected, transport = " + $.connection.hub.transport.name);
    });

    $("#chat-button").click(function () {
        //window.location.href = url.inquiryChat;
        console.log(url.inquiryChat);
        window.location.href = url.inquiryChat;
    });
});

function enableChatButton() {
    $("#chat-online-icon").addClass("fg-green");
    $("#chat-button").addClass("primary").prop('disabled', false);
}

function disableChatButton() {
    $("#chat-online-icon").removeClass("fg-green");
    $("#chat-button").removeClass("primary").prop('disabled', true);
}