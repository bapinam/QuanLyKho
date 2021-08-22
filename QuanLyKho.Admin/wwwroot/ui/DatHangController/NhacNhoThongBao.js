"use strict";

var connectionNhacNho = new signalR.HubConnectionBuilder().withUrl("/nhacNhoHub").build();

// nhận thông điệp bên người nhận
connectionNhacNho.on("ReceiveNhacNho", function (user, message) {
    GetValueThongBao(user, message);
});

// khởi tạo
connectionNhacNho.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});