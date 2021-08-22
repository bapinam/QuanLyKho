using QuanLyKho.Data.Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ViewModels.User
{
    public class UpdateJobStauts
    {
        public Guid Id { set; get; }
        public TinhTrangLamViec TinhTrangLamViec { set; get; }
    }
}