using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.SanPham
{
    public class TaoToanBoSanPham
    {
        [Display(Name = "Hình Ảnh")]
        public IFormFile HinhAnh { get; set; }

        [Display(Name = "Mã số")]
        public string MaSo { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Ten { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Mức tồn tối đa")]
        public long? MucTonCaoNhat { get; set; }

        [Display(Name = "Mức tồn tối thiểu")]
        public long? MucTonThapNhat { get; set; }

        [Display(Name = "Nhắc nhở")]
        public bool NhacNho { get; set; }

        [Display(Name = "Tên đơn vị tính cơ bản")]
        public string TenDVTCoBan { get; set; }

        [Display(Name = "Tên đơn vị tính")]
        public string[] TenDVT { get; set; }

        [Display(Name = "Giá trị chuyển đổi")]
        public long?[] GiaTriDVT { get; set; }

        [Display(Name = "Id loại sản phẩm")]
        public int IdLoaiSanPham { get; set; }
    }
}