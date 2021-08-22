using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.DatHang
{
    public class TimKiemPhieuNhapDaTra : PagingRequestBase
    {
        public string Ngay { get; set; }
        public string TuKhoa { get; set; }
    }
}