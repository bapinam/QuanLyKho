"use strict";

var connectionPhieuCheBien = new signalR.HubConnectionBuilder().withUrl("/nhanPhieuCheBienHub").build();

// nhận thông điệp bên người nhận
connectionPhieuCheBien.on("ReceivePhieuCheBien", function (user, message) {
    GetValueThongBao(user, message);
});

// khởi tạo
connectionPhieuCheBien.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

// gửi thông điệp
//var user = document.getElementById("userInput").value;
//var message = document.getElementById("messageInput").value;
//connection.invoke("SendMessage", user, message).catch(function (err) {
//    return console.error(err.toString());
//});