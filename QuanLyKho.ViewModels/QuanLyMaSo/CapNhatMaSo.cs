using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.QuanLyMaSo
{
    public class CapNhatMaSo
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public bool DinhDau { get; set; }
        public MaLoai MaLoai { get; set; }
    }
}