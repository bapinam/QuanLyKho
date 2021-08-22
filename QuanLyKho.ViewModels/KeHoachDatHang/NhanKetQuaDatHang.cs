using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.KeHoachDatHang
{
    public class NhanKetQuaDatHang
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public string NgayBatDauDuKien { get; set; }
        public string NgayKetThucDuKien { get; set; }
        public string TrangThai { get; set; }
        public bool NhanKeHoach { get; set; }
        public string NguoiNhan { get; set; }
        public long IdThongBao { get; set; }
        public string ThongDiep { get; set; }
        public bool GiaoChoNguoiKhac { get; set; }
        public List<ThongBaoModel> ThongBaoModelOld { get; set; }
        public List<ThongBaoModel> ThongBaoModelNew { get; set; }
    }
}