using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CongThuc
{
    public class ListBundleNguyenVatLieu
    {
        public NhomLoaiNVL? NhomLoaiNVL { set; get; }
        public int LoaiNguyenVatLieu { set; get; }
        public string TuKhoaNVL { set; get; }
    }
}