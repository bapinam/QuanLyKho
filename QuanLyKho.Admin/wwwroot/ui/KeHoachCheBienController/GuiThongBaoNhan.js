"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/nhanKeHoachCheBienHub").build();

// nhận thông điệp bên người nhận
connection.on("ReceiveMessage", function (user, message) {
    GetValueThongBao(user, message);
});

// khởi tạo
connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

// gửi thông điệp
//var user = document.getElementById("userInput").value;
//var message = document.getElementById("messageInput").value;
//connection.invoke("SendMessage", user, message).catch(function (err) {
//    return console.error(err.toString());
//});