using System;

namespace QuanLyKho.ViewModels.ChiMuc
{
    public class CapNhatChiMuc
    {
        public int Id { get; set; }
        public int[] IdNgayTuChon { get; set; }
        public DateTime[] NgayNghiTuChon { get; set; }
        public DateTime[] NgayTuChonCapNhat { get; set; }
        public string[] NgayNghiMacDinhCapNhat { get; set; }
    }
}