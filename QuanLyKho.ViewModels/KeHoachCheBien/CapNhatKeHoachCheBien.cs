using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachCheBien
{
    public class CapNhatKeHoachCheBien
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public string NgayDuKien { get; set; }
        public string GhiChu { get; set; }
        public long[] IdChiTiet { get; set; }
        public int[] SoLuong { get; set; }
        public string[] DonViTinh { get; set; }
        public string[] GhiChuCheBien { get; set; }
        public int[] IdCongThuc { get; set; }
        public int[] SoLuongThem { get; set; }
        public string[] DonViTinhThem { get; set; }
        public string[] GhiChuCheBienThem { get; set; }
        public int[] IdCongThucThem { get; set; }
        public Guid IdNguoiNhan { get; set; }
    }
}