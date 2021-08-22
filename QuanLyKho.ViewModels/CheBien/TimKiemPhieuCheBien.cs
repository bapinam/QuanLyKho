using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CheBien
{
    public class TimKiemPhieuCheBien : PagingRequestBase
    {
        public string Ngay { get; set; }
        public string TuKhoa { get; set; }
        public int TrangThai { get; set; }
    }
}