namespace QuanLyKho.Data.Entities
{
    public class ChiTietHoaDon
    {
        public long Id { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public int GiamGia { get; set; }

        public int IdNguyenVatLieu { get; set; }
        public long IdHoaDon { get; set; }
        public HoaDon HoaDon { get; set; }
        public NguyenVatLieu NguyenVatLieu { get; set; }
    }
}