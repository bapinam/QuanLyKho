using System;

namespace QuanLyKho.ViewModels.ChiMuc
{
    public class ModelChimuc
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int SoNgayLamViec { get; set; }

        public string TrangThaiNgay
        {
            get
            {
                return this.iNgayTonTai(this.NgayBatDau, this.NgayKetThuc);
            }
        }

        private string iNgayTonTai(DateTime ngay1, DateTime ngay2)
        {
            var ngayHienTai = DateTime.Now;
            if (ngay1 <= ngayHienTai && ngay2 >= ngayHienTai)
            {
                return "Đang sử dụng";
            }

            if (ngay1 <= ngayHienTai && ngay2 <= ngayHienTai)
            {
                return "Đã sử dụng";
            }

            return "Chưa sử dụng";
        }
    }
}