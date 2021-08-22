using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CheBien
{
    public class NhanKetQuaThongBaoCB
    {
        public long Id { get; set; }
        public long IdKeHoach { get; set; }
        public string MaPhieu { get; set; }
        public string MaKeHoach { get; set; }
        public string NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiNhan { get; set; }
        public string UserName { get; set; }
        public string TrangThai { get; set; }
        public bool GiaTri { get; set; }
        public long IdThongBao { get; set; }
        public string ThongDiep { get; set; }
    }
}