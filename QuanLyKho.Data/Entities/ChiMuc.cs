using System;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class ChiMuc
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public string NgayNghiMacDinh { get; set; }
        public int SoNgayLamViec { get; set; }
        public List<HoaDon> HoaDons { get; set; }
        public List<ThongBao> ThongBaos { get; set; }
        public List<KeHoachCheBien> KeHoachCheBiens { get; set; }
        public List<PhieuCheBien> PhieuCheBiens { get; set; }
        public List<KeHoachDatHang> KeHoachDatHangs { get; set; }
        public List<NgayNghiTuChon> NgayNghiTuChons { get; set; }
    }
}