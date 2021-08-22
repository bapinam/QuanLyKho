using QuanLyKho.ViewModels.ChiMuc;
using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.ChiMucApiClient
{
    public interface IChiMucApiClient
    {
        Task<ApiResult<ModelChiMucGetOne>> Create(TaoChiMuc bundle);

        Task<ApiResult<string>> GetName(string ten);

        Task<ApiResult<PagedResult<ModelChimuc>>> GetAll(GetAllChiMuc bundle);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<UpdateChiMucModel>> DeleteNgayNghi(int id);

        Task<ApiResult<UpdateChiMucModel>> Update(CapNhatChiMuc bundle);

        Task<ApiResult<ChiMucTheoId>> GetChiMucTheoId(int id);

        Task<ApiResult<int>> iNgayLamViec();
    }
}