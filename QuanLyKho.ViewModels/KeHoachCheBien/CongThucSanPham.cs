using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachCheBien
{
    public class CongThucSanPham
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public bool TrangThaiTonKho { get; set; }
        public List<CongThucSanPhamKH> CongThucSanPhamKHs { get; set; }
    }

    public class CongThucSanPhamKH
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
    }
}