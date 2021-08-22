using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Data.Entities
{
    public class QuanLyMaSo
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public bool DinhDau { get; set; }
        public MaLoai MaLoai { get; set; }
        public long ViTri { set; get; }
    }
}