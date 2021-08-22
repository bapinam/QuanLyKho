using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuanLyKho.ViewModels.User
{
    public class UserUpdatePassword
    {
        [Display(Name = "Tên tài khoản")]
        [DataType(DataType.Password)]
        public string Ten { get; set; }

        [Display(Name = "Mật khẩu mới củ")]
        [DataType(DataType.Password)]
        public string MatKhauCu { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        [DataType(DataType.Password)]
        public string XacNhanMatKhau { get; set; }
    }
}