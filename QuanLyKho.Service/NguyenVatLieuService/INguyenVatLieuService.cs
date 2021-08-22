using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.NguyenVatLieuService
{
    public interface INguyenVatLieuService
    {
        Task<ApiResult<PagedResult<NguyenVatLieuModel>>> GetNguyenVatLieuPaging(GetNguyenVatLieuPaging bundle);

        Task<ApiResult<NguyenVatLieuModel>> Create(TaoNguyenVatLieu bundle);

        Task<ApiResult<string>> CapNhatHinhAnh(int id, CapNhatHinhAnhNVL bundle);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<ApiResult<NguyenVatLieuShowAll>> GetShowAllNguyenVatLieu(int id);

        Task<ApiResult<NguyenVatLieuModel>> Update(CapNhatNguyenVatLieu bundle);

        Task<ApiResult<NguyenVatLieuModel>> GetByUpdateNguyenVatLieu(int id);

        Task<ApiResult<bool>> UpdateNhacNho(NhacNhoModel bundle);

        Task<ApiResult<NhacNhoModel>> GetNhacNho(int id);
    }
}