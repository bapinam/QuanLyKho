using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Data.Entities
{
    public class ChiTietCheBien
    {
        public long Id { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaThem { get; set; }
        public string DonViTinh { get; set; }
        public TrangThaiChiTiet TrangThaiChiTiet { get; set; }
        public string GhiChu { get; set; }

        public int IdCongThuc { get; set; }
        public long IdKeHoachCheBien { get; set; }
        public CongThuc CongThuc { get; set; }
        public KeHoachCheBien KeHoachCheBien { get; set; }
    }
}