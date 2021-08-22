namespace QuanLyKho.Data.Entities
{
    public class ChiTietPhieuCheBien
    {
        public long Id { get; set; }
        public int SoLuong { get; set; }
        public string DonViTinh { get; set; }
        public int IdCongThuc { get; set; }
        public CongThuc CongThuc { get; set; }

        public long IdPhieu { get; set; }
        public PhieuCheBien PhieuCheBien { get; set; }
    }
}