using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CheBien
{
    public class TaoPhieuCheBien
    {
        public int IdChiMuc { get; set; }
        public long[] IdChiTiet { get; set; }
        public int[] IdCongThuc { get; set; }
        public int[] SoLuong { get; set; }
        public string[] DonViTinh { get; set; }
        public long IdKeHoach { get; set; }
        public string NguoiTao { get; set; }
        public string MaSo { get; set; }
    }
}