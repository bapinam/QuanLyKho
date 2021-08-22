using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CongThuc
{
    public class SanPhamCongThuc
    {
        [Display(Name = "Id")]
        public int Id { set; get; }

        [Display(Name = "Mã Số")]
        public string MaSo { set; get; }

        [Display(Name = "Tên")]
        public string Ten { set; get; }

        [Display(Name = "Hình ảnh")]
        public string HinhAnh { set; get; }

        [Display(Name = "Tên công thức")]
        public string TenCongThuc { set; get; }

        [Display(Name = "Tên loại sản phẩm")]
        public string TenLoaiSanPham { set; get; }
    }
}