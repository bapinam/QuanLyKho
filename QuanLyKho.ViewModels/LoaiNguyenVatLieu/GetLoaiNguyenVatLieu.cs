using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.LoaiNguyenVatLieu
{
    public class GetLoaiNguyenVatLieu : PagingRequestBase
    {
        public string TuKhoa { get; set; }
        public NhomLoaiNVL? NhomLoai { get; set; }
    }
}