using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class CongThuc
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public bool DinhDau { get; set; }
        public string GhiChu { get; set; }

        public int IdSanPham { get; set; }
        public SanPham SanPham { get; set; }

        public List<ChiTietCheBien> ChiTietCheBiens { get; set; }
        public List<ChiTietCongThuc> ChiTietCongThucs { get; set; }
        public List<ChiTietPhieuCheBien> ChiTietPhieuCheBiens { get; set; }
    }
}