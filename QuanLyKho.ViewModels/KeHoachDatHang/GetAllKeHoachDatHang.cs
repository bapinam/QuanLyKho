using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class GetAllKeHoachDatHang : PagingRequestBase
    {
        public string TuKhoa { get; set; }
        public string NgayDuKien { get; set; }
        public int TrangThai { get; set; }
    }
}