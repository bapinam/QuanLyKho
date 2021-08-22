using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NhomSanPham
{
    public class TaoNhomSanPham
    {
        [Display(Name = "Mã số")]
        public string MaSo { get; set; }

        [Display(Name = "Tên nhóm")]
        public string Ten { get; set; }
    }
}