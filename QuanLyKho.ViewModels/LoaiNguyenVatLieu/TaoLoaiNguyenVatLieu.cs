using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.LoaiNguyenVatLieu
{
    public class TaoLoaiNguyenVatLieu
    {
        [Display(Name = "Mã số")]
        public string MaSo { get; set; }

        [Display(Name = "Tên loại")]
        public string Ten { get; set; }

        [Display(Name = "Nhóm loại")]
        public NhomLoaiNVL NhomLoaiNVL { get; set; }
    }
}