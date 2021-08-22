using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.DatHang
{
    public class DanhSachNguyenLieuDatHang
    {
        public int IdNguyenVatLieu { get; set; }
        public long IdChiTiet { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongDaThem { get; set; }
        public string DonViTinh { get; set; }
    }
}