using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class NguyenVatLieu
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public long SoLuong { get; set; }
        public long MucTonThapNhat { get; set; }
        public long? MucTonCaoNhat { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }

        public bool NhacNho { get; set; }

        public int IdLoaiNVL { get; set; }
        public LoaiNguyenVatLieu LoaiNguyenVatLieu { get; set; }

        public List<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public List<ChiTietDatHang> ChiTietDatHangs { get; set; }
        public List<DonViTinh> DonViTinhs { get; set; }
        public List<ChiTietCongThuc> ChiTietCongThucs { get; set; }
    }
}