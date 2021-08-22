using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class KeHoachDatHangModel
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
        public List<DanhSachDatHang> DanhSachDatHangs { get; set; }
    }

    public class DanhSachDatHang
    {
        public int IdNguyenVatLieu { get; set; }
        public long IdChiTiet { get; set; }
        public int IdNhaCungCap { get; set; }
        public string TenNguyenVatLieu { get; set; }
        public string MaSoNguyenVatLieu { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public string GhiChu { get; set; }
        public string TrangThai { get; set; }
        public string MaSoNCC { get; set; }
    }
}