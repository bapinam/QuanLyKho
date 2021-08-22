using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NguyenVatLieu
{
    public class CapNhatNguyenVatLieu
    {
        public int Id { get; set; }

        [Display(Name = "Tên nguyên vật liệu")]
        public string Ten { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Id loại nguyên vật liệu")]
        public int IdLoaiNguyenVatLieu { get; set; }
    }
}