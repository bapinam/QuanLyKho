using Microsoft.AspNetCore.Identity;
using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string MaSo { get; set; }
        public string CanCuocCongDan { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string HinhAnh { get; set; }
        public TinhTrangLamViec TinhTrangLamViec { get; set; }
        public LoaiTaiKhoan LoaiTaiKhoan { get; set; }

        public List<KeHoachDatHang> NguoiTaoKeHoachDatHangs { get; set; }
        public List<KeHoachDatHang> NguoiNhanKeHoachDatHangs { get; set; }
        public List<KeHoachCheBien> NguoiTaoKeHoachCheBiens { get; set; }
        public List<KeHoachCheBien> NguoiNhanKeHoachCheBiens { get; set; }
        public List<ThongBao> NguoiNhanThongBaos { get; set; }
        public List<ThongBao> NguoiGuiThongBaos { get; set; }
        public List<HoaDon> HoaDons { get; set; }
        public List<PhieuCheBien> NguoiTaoPhieus { get; set; }
        public List<PhieuCheBien> NguoiDuyetPhieus { get; set; }
    }
}