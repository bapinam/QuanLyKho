using QuanLyKho.ApiIntegration.ThongBaoApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.HangFire
{
    public class DeleteNotify : IDeleteNotify
    {
        private readonly IThongBaoApiClient _thongBaoApiClient;

        public DeleteNotify(IThongBaoApiClient thongBaoApiClient)
        {
            _thongBaoApiClient = thongBaoApiClient;
        }

        public async Task<string> Delete()
        {
            var reuslt = await _thongBaoApiClient.DeleteAll();
            if (reuslt.IsSuccessed)
            {
                Console.WriteLine(reuslt.ResultObj);
            }
            else
            {
                Console.WriteLine(reuslt.ResultObj);
            }
            throw new NotImplementedException();
        }
    }
}