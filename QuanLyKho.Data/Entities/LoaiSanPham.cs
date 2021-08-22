using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class LoaiSanPham
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }

        public List<SanPham> SanPhams { get; set; }
        public int IdNhomSanPham { get; set; }
        public NhomSanPham NhomSanPham { get; set; }
    }
}