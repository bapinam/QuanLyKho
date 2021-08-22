using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.SignalRChat
{
    public class NhanPhieuCheBien : Hub
    {
        public async Task SendPhieuCheBien(string user, object message)
        {
            await Clients.All.SendAsync("ReceivePhieuCheBien", user, message);
        }
    }
}