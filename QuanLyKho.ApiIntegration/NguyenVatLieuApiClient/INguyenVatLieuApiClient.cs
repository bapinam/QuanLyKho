using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.NguyenVatLieuApiClient
{
    public interface INguyenVatLieuApiClient
    {
        Task<ApiResult<PagedResult<NguyenVatLieuModel>>> GetNguyenVatLieuPaging(GetNguyenVatLieuPaging bundle);

        Task<ApiResult<NguyenVatLieuModel>> Create(TaoToanBoNguyenVatLieu bundle);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<ApiResult<NguyenVatLieuModel>> Update(CapNhatToanBoNguyenVatLieu bundle);

        Task<ApiResult<NguyenVatLieuModel>> GetByUpdateNguyenVatLieu(int id);

        Task<ApiResult<NguyenVatLieuShowAll>> GetShowAllNguyenVatLieu(int id);

        Task<ApiResult<bool>> UpdateNhacNho(NhacNhoModel bundle);

        Task<ApiResult<NhacNhoModel>> GetNhacNho(int id);
    }
}