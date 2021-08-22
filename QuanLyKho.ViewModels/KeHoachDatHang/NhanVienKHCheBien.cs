using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class NhanVienKHDatHang
    {
        public Guid Id { get; set; }
        public string MaSo { get; set; }
        public string CanCuocCongDan { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
    }
}