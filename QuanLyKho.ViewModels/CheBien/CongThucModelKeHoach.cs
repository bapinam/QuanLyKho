using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CheBien
{
    public class CongThucModelKeHoach
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public long IdChiTiet { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaThem { get; set; }
        public long IdCongThuc { get; set; }
        public string MaCongThuc { get; set; }
        public List<DanhSachNguyenVatLieu> DanhSachNguyenVatLieus { get; set; }
    }

    public class DanhSachNguyenVatLieu
    {
        public int IdNguyenVatLieu { get; set; }
        public string TenNguyenVatLieu { get; set; }
        public string MaNguyenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuongCanCT { get; set; }
        public long SoLuongHienCoNVL { get; set; }
        public long GiaTriChuyenDoi { get; set; }
    }
}