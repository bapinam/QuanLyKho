using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CheBien
{
    public class ChiTietPhieuCheBienModel
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string MaKeHoach { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiDuyet { get; set; }
        public string NgayTao { get; set; }
        public string NgayHoanThanh { get; set; }
        public string NguoiNhan { get; set; }
        public List<DanhSachPhieuChiTietCheBien> DanhSachPhieuChiTietCheBiens { get; set; }
    }

    public class DanhSachPhieuChiTietCheBien
    {
        public long Id { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public string MaCongThuc { get; set; }
        public string TenSanPham { get; set; }
        public string MaSanPham { get; set; }
    }
}