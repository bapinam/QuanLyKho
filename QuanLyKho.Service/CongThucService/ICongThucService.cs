using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyKho.ViewModels.CongThuc;
using QuanLyKho.Data.Extensions.Enums;

namespace QuanLyKho.Service.CongThucService
{
    public interface ICongThucService
    {
        Task<ApiResult<PagedResult<SanPhamCongThuc>>> GetCongThucSPPaging(GetCongThucSPPaging bundle);

        Task<ApiResult<GetListIdCongThuc>> GetListIdCongThuc(int id);

        Task<List<GetListCongThuc>> GetListCongThuc(int id);

        Task<ApiResult<bool>> iTen(string ten, int idSP, int? id);

        Task<ApiResult<bool>> iNguyenVatLieu(int idCongThuc, int idNguyenVatLieu);

        Task<List<GetLoaiNguyenVatLieu>> GetLoaiNguyenVatLieu(NhomLoaiNVL? group);

        Task<List<GetListNguyenVatLieu>> GetListNguyenVatLieu(ListBundleNguyenVatLieu bundle);

        Task<List<GetListDVTSanPham>> GetListDonViTinhs(int id);

        Task<ApiResult<GetTaoCongThuc>> Create(TaoCongThuc bundle);

        Task<ApiResult<string>> UpdateDinhDauCongThuc(CapNhatDinhDauCongThuc bundle);

        Task<ApiResult<GetTaoCongThuc>> Update(CapNhatCongThuc bundle);

        Task<ApiResult<string>> Delete(int id, int idSanPham);

        Task<ApiResult<bool>> DeleteNguyenVatLieu(long id);
    }
}