using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NguyenVatLieu
{
    public class NguyenVatLieuModel
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

        [Display(Name = "Mức tồn tối thiểu")]
        public long MucTonThapNhat { get; set; }

        [Display(Name = "Mức tồn tối đa")]
        public long? MucTonCaoNhat { get; set; }

        [Display(Name = "Mô tả")]
        public string MoTa { get; set; }

        [Display(Name = "Số lượng")]
        public int PhanTramSoLuong
        {
            get
            {
                return PhanTramSL(this.MucTonCaoNhat, this.SoLuong);
            }
        }

        private int PhanTramSL(long? max, long soLuong)
        {
            if (max != null)
            {
                var result = (int)(((double)soLuong / (double)max) * 100);
                return result;
            }
            else
            {
                return 0;
            }
        }
    }
}