using QuanLyKho.ViewModels.ChiMuc;
using QuanLyKho.ViewModels.Common;
using System;
using System.Threading.Tasks;

namespace QuanLyKho.Service.ChiMucService
{
    public interface IChiMucService
    {
        Task<ApiResult<string>> GetName(string ten);

        Task<ApiResult<ModelChiMucGetOne>> Create(TaoChiMuc bundle);

        Task<ApiResult<PagedResult<ModelChimuc>>> GetAll(GetAllChiMuc bundle);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<UpdateChiMucModel>> DeleteNgayNghi(int id);

        Task<ApiResult<UpdateChiMucModel>> Update(CapNhatChiMuc bundle);

        Task<ApiResult<bool>> iNgayTonTai(DateTime ngay, int? id);

        Task<ApiResult<int>> iNgayLamViec();

        Task<ApiResult<ChiMucTheoId>> GetChiMucTheoId(int id);
    }
}