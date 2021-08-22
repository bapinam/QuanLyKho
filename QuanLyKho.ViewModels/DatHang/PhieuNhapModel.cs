using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.DatHang
{
    public class PhieuNhapModel
    {
        public long Id { get; set; }
        public string MaLuuTru { get; set; }
        public string MaKeHoach { get; set; }
        public string NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string TrangThai { get; set; }

        public long IdKeHoach { get; set; }
        public string TrangThaiHoanKH { get; set; }

        public List<NhacNhoSoLuong> NhacNhoSoLuongs { get; set; }
    }

    public class NhacNhoSoLuong
    {
        public long IdThongBao { get; set; }
        public string Ten { get; set; }
        public string DuongDan { get; set; }
        public string NguoiNhan { get; set; }
    }
}