using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.SanPham
{
    public class CapNhatToanBoSanPham
    {
        [Display(Name = "Id sản phẩm")]
        public int IdSanPham { get; set; }

        [Display(Name = "Hình Ảnh")]
        public IFormFile HinhAnh { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Ten { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Id loại sản phẩm")]
        public int IdLoaiSanPham { get; set; }
    }
}