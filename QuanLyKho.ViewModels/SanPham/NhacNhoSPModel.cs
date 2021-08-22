using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.SanPham
{
    public class NhacNhoSPModel
    {
        public int Id { get; set; }

        [Display(Name = "Mức tồn tối thiểu")]
        public long? MucTonThapNhat { get; set; }

        [Display(Name = "Mức tồn tối đa")]
        public long? MucTonCaoNhat { get; set; }

        [Display(Name = "Nhắc nhở")]
        public bool NhacNho { get; set; }
    }
}