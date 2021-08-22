using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.ThongBao
{
    public class SetThongBao : PagingRequestBase
    {
        public string TuKhoa { get; set; }
        public string NgayNhanThongBao { get; set; }
        public int TrangThaiXem { get; set; }
        public string UserName { get; set; }
    }
}