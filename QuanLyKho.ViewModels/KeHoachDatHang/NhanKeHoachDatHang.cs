using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class NhanKeHoachDatHang
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public string NguoiTao { get; set; }
        public string NgayDuKien { get; set; }
        public string TrangThai { get; set; }
        public string TinhTrang { get; set; }
    }
}