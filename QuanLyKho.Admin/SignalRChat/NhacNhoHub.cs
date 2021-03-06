using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.SignalRChat
{
    public class NhacNhoHub : Hub
    {
        public async Task SendNhacNho(string user, object message)
        {
            await Clients.All.SendAsync("ReceiveNhacNho", user, message);
        }
    }
}