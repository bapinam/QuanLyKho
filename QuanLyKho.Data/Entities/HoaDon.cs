using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class HoaDon
    {
        public long Id { get; set; }
        public string SoHoaDon { get; set; }
        public string MaLuuTru { get; set; }
        public DateTime NgayMua { get; set; }
        public DateTime NgayTao { get; set; }
        public int ThueSuat { get; set; }
        public ThanhToanHoaDon ThanhToanHoaDon { get; set; }
        public decimal SoTienDaTra { get; set; }
        public decimal TongTien { get; set; }

        public int IdNCC { get; set; }
        public long IdKeHoach { get; set; }
        public KeHoachDatHang KeHoachDatHang { get; set; }
        public NhaCungCap NhaCungCap { get; set; }
        public List<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public AppUser NguoiTao { get; set; }
        public Guid IdNguoiTao { get; set; }

        public int IdChiMuc { get; set; }
        public ChiMuc ChiMuc { get; set; }
    }
}