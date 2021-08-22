using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class CapNhatKeHoachDatHang
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public string NgayDuKien { get; set; }
        public string GhiChu { get; set; }
        public long[] IdChiTiet { get; set; }
        public int[] SoLuong { get; set; }
        public string[] DonViTinh { get; set; }
        public string[] GhiChuDatHang { get; set; }
        public int[] IdNguyenVatLieu { get; set; }
        public int?[] IdNhaCungCap { get; set; }
        public int[] SoLuongThem { get; set; }
        public string[] DonViTinhThem { get; set; }
        public string[] GhiChuDatHangThem { get; set; }
        public int[] IdNguyenVatLieuThem { get; set; }
        public int?[] IdNhaCungCapThem { get; set; }
        public Guid IdNguoiNhan { get; set; }
    }
}