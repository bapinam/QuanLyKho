using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.CongThuc
{
    public class GetListIdCongThuc
    {
        public int Id { set; get; }
        public string MaSo { set; get; }
        public string Ten { set; get; }
        public string GhiChu { set; get; }
        public bool DinhDau { set; get; }

        public List<IngredientCongThuc> IngredientCongThucs { set; get; }
    }
}