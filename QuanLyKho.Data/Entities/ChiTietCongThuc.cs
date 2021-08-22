namespace QuanLyKho.Data.Entities
{
    public class ChiTietCongThuc
    {
        public long Id { get; set; }
        public int SoLuong { get; set; }
        public string DonViTinh { get; set; }

        public int IdCongThuc { get; set; }
        public CongThuc CongThuc { get; set; }

        public int IdNguyenVatLieu { get; set; }
        public NguyenVatLieu NguyenVatLieu { get; set; }
    }
}