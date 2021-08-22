using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class TaoKeHoachDatHang
    {
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public string NgayDuKien { get; set; }
        public string GhiChu { get; set; }
        public int[] SoLuong { get; set; }
        public string[] DonViTinh { get; set; }
        public string[] GhiChuDatHang { get; set; }
        public int[] IdNguyenVatLieu { get; set; }
        public int?[] IdNhaCungCap { get; set; }
        public string NguoiTao { get; set; }
        public Guid IdNguoiNhan { get; set; }
        public int IdChiMuc { get; set; }
    }
}