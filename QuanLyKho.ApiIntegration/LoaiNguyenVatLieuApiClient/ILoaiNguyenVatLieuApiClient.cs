using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.LoaiNguyenVatLieuApiClient
{
    public interface ILoaiNguyenVatLieuApiClient
    {
        Task<ApiResult<LoaiNguyenVatLieuModel>> Create(TaoLoaiNguyenVatLieu bundle);

        Task<ApiResult<bool>> Update(CapNhatLoaiNguyenVatLieu bundle);

        Task<ApiResult<PagedResult<LoaiNguyenVatLieuModel>>> GetLoaiNguyenVatLieuPaging(GetLoaiNguyenVatLieu bundle);

        Task<ApiResult<LoaiNguyenVatLieuModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<List<GetTenLoaiNVL>> GetTenLoaiNVL();
    }
}