using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CheBien
{
    public class PhieuCheBienModel
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string MaKeHoach { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiNhan { get; set; }
        public string TrangThai { get; set; }
        public string NgayTao { get; set; }
    }
}