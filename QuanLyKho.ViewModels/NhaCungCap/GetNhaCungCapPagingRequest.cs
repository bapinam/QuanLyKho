using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.NhaCungCap
{
    public class GetNhaCungCapPagingRequest : PagingRequestBase
    {
        public string TuKhoa { get; set; }
    }
}