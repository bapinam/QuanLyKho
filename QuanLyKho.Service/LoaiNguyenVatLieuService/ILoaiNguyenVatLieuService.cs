using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.LoaiNguyenVatLieuService
{
    public interface ILoaiNguyenVatLieuService
    {
        Task<ApiResult<LoaiNguyenVatLieuModel>> Create(TaoLoaiNguyenVatLieu bundle);

        Task<ApiResult<bool>> Update(int id, CapNhatLoaiNguyenVatLieu bundle);

        Task<ApiResult<PagedResult<LoaiNguyenVatLieuModel>>> GetLoaiNguyenVatLieuPaging(GetLoaiNguyenVatLieu bundle);

        Task<ApiResult<LoaiNguyenVatLieuModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<List<GetTenLoaiNVL>> GetTenLoaiNVL();
    }
}