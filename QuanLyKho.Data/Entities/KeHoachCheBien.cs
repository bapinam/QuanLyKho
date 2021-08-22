using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class KeHoachCheBien
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayBatDauDuKien { get; set; }
        public DateTime NgayKetThucDuKien { get; set; }
        public string GhiChu { get; set; }
        public TrangThaiKeHoach TrangThai { set; get; }
        public bool NhanKeHoach { get; set; }
        public bool LenDatHang { get; set; }
        public List<ChiTietCheBien> ChiTietCheBiens { get; set; }
        public List<PhieuCheBien> PhieuCheBiens { get; set; }
        public AppUser NguoiTao { get; set; }
        public Guid IdNguoiTao { get; set; }
        public AppUser NguoiNhan { get; set; }
        public Guid IdNguoiNhan { get; set; }

        public int IdChiMuc { get; set; }
        public ChiMuc ChiMuc { get; set; }
    }
}