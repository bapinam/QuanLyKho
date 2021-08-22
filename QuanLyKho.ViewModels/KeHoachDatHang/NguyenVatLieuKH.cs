using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class NguyenVatLieuKH
    {
        public int Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public long SoLuong { get; set; }
        public bool TrangThaiTonKho { get; set; }
    }
}