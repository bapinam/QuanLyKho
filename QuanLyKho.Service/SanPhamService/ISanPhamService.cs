using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.SanPhamService
{
    public interface ISanPhamService
    {
        Task<ApiResult<PagedResult<SanPhamModel>>> GetSanPhamPaging(GetSanPhamPaging bundle);

        Task<ApiResult<SanPhamModel>> Create(TaoSanPham bundle);

        Task<ApiResult<string>> CapNhatHinhAnh(int id, CapNhatHinhAnhSP bundle);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<ApiResult<SanPhamShowAll>> GetShowAllSanPham(int id);

        Task<ApiResult<SanPhamModel>> Update(CapNhatSanPham bundle);

        Task<ApiResult<SanPhamModel>> GetByUpdateSanPham(int id);

        Task<ApiResult<bool>> UpdateNhacNho(NhacNhoSPModel bundle);

        Task<ApiResult<NhacNhoSPModel>> GetNhacNho(int id);
    }
}