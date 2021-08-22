using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLyKho.ViewModels.User
{
    public class GetByIdListUser
    {
        public Guid Id { get; set; }

        [Display(Name = "Mã Số")]
        public string MaSo { get; set; }

        [Display(Name = "Chứng minh thư")]
        public string CanCuocCongDan { get; set; }

        [Display(Name = "Họ")]
        public string Ho { get; set; }

        [Display(Name = "Tên")]
        public string Ten { get; set; }

        [Display(Name = "Giới tính")]
        public string GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public string NgaySinh { get; set; }

        [Display(Name = "Ngày sinh cập nhật")]
        [DataType(DataType.Date)]
        public string NgaySinhCapNhat { get; set; }

        [Display(Name = "Số điện thoại")]
        public string SoDienThoai { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }

        public IList<string> VaiTro { get; set; }

        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Tình Trạng Làm")]
        public string TinhTrangLamViec { get; set; }

        [Display(Name = "Hình ảnh")]
        public string DuongDanHinhAnh { get; set; }
    }
}