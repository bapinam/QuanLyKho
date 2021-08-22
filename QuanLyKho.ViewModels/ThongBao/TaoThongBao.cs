using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.ThongBao
{
    public class TaoThongBao
    {
        public string Ten { get; set; }
        public string DuongDan { get; set; }
        public string NguoiNhan { get; set; }
        public int IdChiMuc { get; set; }
        public LoaiThongBao LoaiThongBao { get; set; }
        public string MaKeHoach { get; set; }
        public Guid IdNguoiGui { get; set; }
    }
}