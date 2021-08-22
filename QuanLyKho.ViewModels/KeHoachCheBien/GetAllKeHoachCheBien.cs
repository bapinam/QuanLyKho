using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachCheBien
{
    public class GetAllKeHoachCheBien : PagingRequestBase
    {
        public string TuKhoa { get; set; }
        public string NgayDuKien { get; set; }
        public int TrangThai { get; set; }
    }
}