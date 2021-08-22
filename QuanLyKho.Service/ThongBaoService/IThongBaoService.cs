using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.ThongBaoService
{
    public interface IThongBaoService
    {
        Task<ApiResult<long>> Create(TaoThongBao bundle);

        Task<ApiResult<bool>> Delete(long id);

        Task<ApiResult<string>> DeleteAll();

        Task<ApiResult<PagedResult<ThongBaoModel>>> GetThongBaoPaging(SetThongBao bundle);

        Task<List<ThongBaoModel>> GetTopThongBao(string userName);

        Task<ApiResult<bool>> UpdateDaXem(long id);
    }
}