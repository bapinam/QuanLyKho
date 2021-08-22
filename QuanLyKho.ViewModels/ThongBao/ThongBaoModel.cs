using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.ThongBao
{
    public class ThongBaoModel
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public string DuongDan { get; set; }
        public string Xem { get; set; }
        public int SoNgayNhan { get; set; }
        public string NgayNhan { get; set; }
        public string NguoiNhan { get; set; }
        public string NguoiGui { get; set; }
    }
}