using QuanLyKho.Data.Extensions.Enums;
using System;

namespace QuanLyKho.Data.Entities
{
    public class ThongBao
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public string DuongDan { get; set; }
        public bool Xem { get; set; }
        public DateTime NgayNhan { get; set; }
        public LoaiThongBao LoaiThongBao { get; set; }
        public string MaKeHoach { get; set; }
        public AppUser NguoiNhan { get; set; }
        public Guid IdNguoiNhan { get; set; }
        public AppUser NguoiGui { get; set; }
        public Guid IdNguoiGui { get; set; }
        public int IdChiMuc { get; set; }
        public ChiMuc ChiMuc { get; set; }
    }
}