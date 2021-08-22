using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.DonViTinh
{
    public class CapNhatDonViTinh
    {
        public long Id { get; set; }
        public string Ten { get; set; }
        public long GiaTriChuyenDoi { get; set; }
        public bool CoBan { get; set; }
    }
}