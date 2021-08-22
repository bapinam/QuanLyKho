using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.LoaiSanPham
{
    public class GetTenLoaiSP
    {
        public int Id { get; set; }

        [Display(Name = "Ten")]
        public string Ten { get; set; }
    }
}