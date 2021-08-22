using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.SignalRChat
{
    public class NhanKeHoachDatHangHub : Hub
    {
        public async Task SendNhanKeHoachDatHang(string user, object message)
        {
            await Clients.All.SendAsync("NhanKeHoachDatHang", user, message);
        }
    }
}