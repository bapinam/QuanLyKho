using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CongThuc
{
    public class GetCongThucSPPaging : PagingRequestBase
    {
        public string TuKhoa { get; set; }
        public int IdLoaiSanPham { get; set; }
    }
}