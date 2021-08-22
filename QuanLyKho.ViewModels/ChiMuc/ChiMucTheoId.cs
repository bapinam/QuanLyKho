using System.Collections.Generic;

namespace QuanLyKho.ViewModels.ChiMuc
{
    public class ChiMucTheoId
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string NgayBatDau { get; set; }
        public string NgayKetThuc { get; set; }
        public string[] NgayNghiMacDinh { get; set; }
        public List<NgayTuChonModel> NgayNghiTuChon { get; set; }
    }

    public class NgayTuChonModel
    {
        public int Id { get; set; }
        public string Ngay { get; set; }
    }
}