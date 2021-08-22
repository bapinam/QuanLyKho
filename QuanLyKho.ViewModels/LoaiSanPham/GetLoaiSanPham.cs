using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.LoaiSanPham
{
    public class GetLoaiSanPham : PagingRequestBase
    {
        public string TuKhoa { get; set; }
        public int IdNhomLoai { get; set; }
    }
}