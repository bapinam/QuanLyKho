"use strict";

var connectionDatHang = new signalR.HubConnectionBuilder().withUrl("/nhanKeHoachDatHangHub").build();

// nhận thông điệp bên người nhận
connectionDatHang.on("NhanKeHoachDatHang", function (user, message) {
    GetValueThongBao(user, message);
});

// khởi tạo
connectionDatHang.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});

// gửi thông điệp
//var user = document.getElementById("userInput").value;
//var message = document.getElementById("messageInput").value;
//connection.invoke("SendMessage", user, message).catch(function (err) {
//    return console.error(err.toString());
//});