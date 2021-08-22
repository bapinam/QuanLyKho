using QuanLyKho.Data.Extensions.Enums;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class LoaiNguyenVatLieu
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public NhomLoaiNVL NhomLoaiNVL { get; set; }
        public List<NguyenVatLieu> NguyenVatLieus { get; set; }
    }
}