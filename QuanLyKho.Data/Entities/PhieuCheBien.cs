using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class PhieuCheBien
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public long IdKeHoach { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayHoanThanh { get; set; }
        public TrangThaiPhieu TrangThaiPhieu { get; set; }
        public KeHoachCheBien KeHoachCheBien { get; set; }
        public AppUser NguoiTao { get; set; }
        public Guid IdNguoiTao { get; set; }
        public AppUser NguoiDuyet { get; set; }
        public Guid? IdNguoiDuyet { get; set; }

        public List<ChiTietPhieuCheBien> ChiTietPhieuCheBiens { get; set; }

        public int IdChiMuc { get; set; }
        public ChiMuc ChiMuc { get; set; }
    }
}