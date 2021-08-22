using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NhaCungCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.NhaCungCapApiClient
{
    public interface INhaCungCapApiClient
    {
        Task<ApiResult<NhaCungCapModel>> Create(TaoNhaCungCap bundle);

        Task<ApiResult<bool>> Update(CapNhatNhaCungCap bundle);

        Task<ApiResult<PagedResult<NhaCungCapModel>>> GetNhaCungCapPaging(GetNhaCungCapPagingRequest bundle);

        Task<ApiResult<NhaCungCapModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iMaThue(string thue, int? id);

        Task<ApiResult<bool>> iEmail(string email, int? id);
    }
}