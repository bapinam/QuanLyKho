using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.QuanLyMaSo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.QuanLyMaSoApiClient
{
    public interface IQuanLyMaSoApiClient
    {
        Task<ApiResult<bool>> Create(TaoMaSo bundle);

        Task<List<LoaiMaSo>> GetAll(MaLoai bundle);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> Update(CapNhatMaSo bundle);

        Task<ApiResult<bool>> iTen(string ten, int? id);
    }
}