using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.DonViTinh
{
    public class TaoDonViTinh
    {
        public string Ten { get; set; }
        public long GiaTriChuyenDoi { get; set; }
        public int IdThem { get; set; }
        public LoaiDongGoi LoaiDongGoi { get; set; }
    }
}