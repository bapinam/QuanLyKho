using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NhomSanPham
{
    public class CapNhatNhomSanPham
    {
        public int Id { get; set; }

        [Display(Name = "Tên nhóm")]
        public string Ten { get; set; }
    }
}