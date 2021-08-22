using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.HangFire
{
    public interface IDeleteNotify
    {
        public Task<string> Delete();
    }
}