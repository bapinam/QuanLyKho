using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class NhaCungCap
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string SoThue { get; set; }
        public string Ten { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        public List<HoaDon> HoaDons { get; set; }
        public List<ChiTietDatHang> ChiTietDatHangs { get; set; }
    }
}