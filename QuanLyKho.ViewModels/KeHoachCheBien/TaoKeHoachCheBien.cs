using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachCheBien
{
    public class TaoKeHoachCheBien
    {
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public string NgayDuKien { get; set; }
        public string GhiChu { get; set; }
        public int[] SoLuong { get; set; }
        public string[] DonViTinh { get; set; }
        public string[] GhiChuCheBien { get; set; }
        public int[] IdCongThuc { get; set; }
        public string NguoiTao { get; set; }
        public Guid IdNguoiNhan { get; set; }
        public int IdChiMuc { get; set; }
    }
}