using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class NhomSanPham
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }

        public List<LoaiSanPham> LoaiSanPhams { get; set; }
    }
}