using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;

namespace QuanLyKho.Data.Entities
{
    public class KeHoachDatHang
    {
        public long Id { get; set; }
        public string MaSo { get; set; }
        public string Ten { get; set; }
        public TrangThaiDatHang DatHang { get; set; } // đã nhận đơn hàng, đang đặt hàng, đã đặt hàng xong, tạm ngưng
        public TrangThaiKeHoach TrangThai { set; get; } // hoàn thành, chưa hoàn thành, bị hủy
        public DateTime NgayTao { get; set; }
        public DateTime NgayBatDauDuKien { get; set; }
        public DateTime NgayKetThucDuKien { get; set; }
        public string GhiChu { get; set; }
        public bool NhanKeHoach { get; set; }

        public List<HoaDon> HoaDons { get; set; }
        public List<ChiTietDatHang> ChiTietDatHangs { get; set; }

        public AppUser NguoiTao { get; set; }
        public Guid IdNguoiTao { get; set; }
        public AppUser NguoiNhan { get; set; }
        public Guid IdNguoiNhan { get; set; }

        public int IdChiMuc { get; set; }
        public ChiMuc ChiMuc { get; set; }
    }
}