using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.DatHang
{
    public class NhapHoaDon
    {
        public long IdKeHoach { get; set; }
        public string MaLuuTru { get; set; }
        public string SoHoaDon { get; set; }
        public int IdNhaCungCap { get; set; }
        public DateTime NgayMua { get; set; }
        public int SoThue { get; set; }
        public string NguoiTao { get; set; }
        public string SoTienDaThanhToan { get; set; }
        public int IdChiMuc { get; set; }
        public double TongTien { get; set; }
        public long[] IdChiTiet { get; set; }
        public int[] IdNguyenVatLieu { get; set; }
        public int[] SoLuong { get; set; }
        public int[] SoLuongDaThem { get; set; }
        public int[] GiamGia { get; set; }
        public string[] DonViTinh { get; set; }
        public double[] DonGia { get; set; }
    }
}