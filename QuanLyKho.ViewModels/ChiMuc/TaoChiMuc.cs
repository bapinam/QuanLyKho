using System;

namespace QuanLyKho.ViewModels.ChiMuc
{
    public class TaoChiMuc
    {
        public string Ten { get; set; }
        public string Ngay { get; set; }
        public string[] NgayNghiMacDinh { get; set; }
        public DateTime[] NgayNghi { get; set; }
    }
}