using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachCheBien
{
    public class KeHoachCheBienModel
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public string NgayTao { get; set; }
        public string NgayDuKienBatDau { get; set; }
        public string NgayDuKienKetThuc { get; set; }
        public string NgayDuKienBatDauEdit { get; set; }
        public string NgayDuKienKetThucEdit { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiNhan { get; set; }
        public string NhanKeHoach { get; set; }
        public string TrangThai { get; set; }
        public string GhiChu { get; set; }
        public Guid IdNguoiNhan { get; set; }
        public List<DanhSachCheBien> DanhSachCheBiens { get; set; }
    }

    public class DanhSachCheBien
    {
        public int IdCongThuc { get; set; }
        public int IdSanPham { get; set; }
        public long IdChiTiet { get; set; }
        public string TenSanPham { get; set; }
        public string MaSoSanPham { get; set; }
        public string MaSoCongThuc { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaThem { get; set; }
        public string GhiChu { get; set; }
        public string TrangThai { get; set; }
    }
}