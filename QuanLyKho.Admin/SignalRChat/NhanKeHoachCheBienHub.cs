using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.SignalRChat
{
    public class NhanKeHoachCheBienHub : Hub
    {
        public async Task SendMessage(string user, object message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}