using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NguyenVatLieu
{
    public class NguyenVatLieuShowAll
    {
        public int Id { get; set; }

        [Display(Name = "Mã số")]
        public string MaSo { get; set; }

        [Display(Name = "Tên nguyên vật liệu")]
        public string Ten { get; set; }

        [Display(Name = "Hình Ảnh")]
        public string HinhAnh { get; set; }

        [Display(Name = "Phần trăm")]
        public long SoLuong { get; set; }

        [Display(Name = "Nhắc nhở")]
        public bool NhacNho { get; set; }

        [Display(Name = "Tên đơn vị tính cơ bản")]
        public string TenDVTCoBan { get; set; }

        [Display(Name = "Tên loại nguyên vật liệu")]
        public string TenLoaiNVL { get; set; }

        [Display(Name = "Id loại nguyên vật liệu")]
        public int IdLoaiNVL { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Danh sách đơn vị tính")]
        public List<DonViTinhCD> DonViTinhCDs { get; set; }
    }

    public class DonViTinhCD
    {
        public long Id { get; set; }

        [Display(Name = "Tên đơn vị tính")]
        public string Ten { get; set; }

        [Display(Name = "Giá trị chuyển đổi")]
        public long GiaTriChuyenDoi { get; set; }

        public long ChuyenDoi { get; set; }
    }
}