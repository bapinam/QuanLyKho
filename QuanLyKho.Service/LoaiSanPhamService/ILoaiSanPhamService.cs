using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.LoaiSanPhamService
{
    public interface ILoaiSanPhamService
    {
        Task<ApiResult<LoaiSanPhamModel>> Create(TaoLoaiSanPham bundle);

        Task<ApiResult<bool>> Update(int id, CapNhatLoaiSanPham bundle);

        Task<ApiResult<PagedResult<LoaiSanPhamModel>>> GetLoaiSanPhamPaging(GetLoaiSanPham bundle);

        Task<ApiResult<LoaiSanPhamModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<List<GetTenLoaiSP>> GetTenLoaiSP();
    }
}