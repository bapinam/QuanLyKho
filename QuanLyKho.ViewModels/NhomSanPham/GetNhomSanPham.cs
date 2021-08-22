using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NhomSanPham
{
    public class GetNhomSanPham : PagingRequestBase
    {
        public string TuKhoa { get; set; }
    }
}