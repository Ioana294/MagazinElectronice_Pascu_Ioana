"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g,
        "&gt;");
    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
    var time = today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
    var encodedMsg = user + " says " + msg + " " + date + " " + time;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
    connection.start().then(function () {
        document.getElementById("sendButton").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });
document.getElementById("sendButton").addEventListener("click", function (event) {
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", "", message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});