using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuanLyKho.ViewModels.User
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string TuKhoa { get; set; }
    }
}