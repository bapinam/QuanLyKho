using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Data.Entities
{
    public class NgayNghiTuChon
    {
        public int Id { get; set; }
        public DateTime NgayNghi { get; set; }
        public ChiMuc ChiMuc { get; set; }
        public int IdChiMuc { get; set; }
    }
}