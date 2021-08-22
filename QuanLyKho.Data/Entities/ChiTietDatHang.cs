using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Data.Entities
{
    public class ChiTietDatHang
    {
        public long Id { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaThem { get; set; }
        public string DonViTinh { get; set; }
        public decimal DonGiaGoiY { get; set; }
        public TrangThaiChiTiet TrangThaiChiTiet { get; set; }
        public string GhiChu { get; set; }

        public int? IdNhaCungCap { get; set; }
        public long IdKeHoachDatHang { get; set; }
        public int IdNguyenVatLieu { get; set; }
        public NhaCungCap NhaCungCap { get; set; }
        public KeHoachDatHang KeHoachDatHang { get; set; }
        public NguyenVatLieu NguyenVatLieu { get; set; }
    }
}