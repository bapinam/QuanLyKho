using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CongThuc
{
    public class TaoCongThuc
    {
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public bool DinhDau { get; set; }
        public string GhiChu { get; set; }

        public int IdSanPham { get; set; }

        public int[] IdNguyenVatLieu { get; set; }

        public int[] SoLuong { get; set; }

        public string[] DonViTinh { get; set; }
    }
}