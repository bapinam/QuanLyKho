using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Data.Entities
{
    public class DonViTinh
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public long GiaTriChuyenDoi { get; set; }
        public bool CoBan { get; set; }
        public LoaiDongGoi LoaiDongGoi { get; set; }

        public int? IdSanPham { get; set; }
        public int? IdNguyenVatLieu { get; set; }
        public NguyenVatLieu NguyenVatLieu { get; set; }
        public SanPham SanPham { get; set; }
    }
}