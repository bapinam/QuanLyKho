using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.CongThuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.CongThucApiClient
{
    public class CongThucApiClient : BaseApiClient, ICongThucApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public CongThucApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<GetTaoCongThuc>> Create(TaoCongThuc bundle)
        {
            var url = $"/api/CongThuc";
            var result = await Create<GetTaoCongThuc, TaoCongThuc>(url, bundle);
            return result;
        }

        public async Task<ApiResult<string>> Delete(int id, int idSanPham)
        {
            var result = await Delete<string>($"/api/CongThuc/{id}/{idSanPham}");
            return result;
        }

        public async Task<ApiResult<bool>> DeleteNguyenVatLieu(long id)
        {
            var result = await Delete<bool>($"/api/CongThuc/nguyenvatlieu/{id}");
            return result;
        }

        public async Task<ApiResult<PagedResult<SanPhamCongThuc>>> GetCongThucSPPaging(GetCongThucSPPaging bundle)
        {
            var url = $"/api/CongThuc/paging?pageIndex=" +
                                        $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}" +
                                        $"&idLoaiSanPham={bundle.IdLoaiSanPham}";
            var result = await GetAsync<ApiResult<PagedResult<SanPhamCongThuc>>>(url);
            return result;
        }

        public async Task<List<GetListCongThuc>> GetListCongThuc(int id)
        {
            var url = $"/api/CongThuc/" + $"{id}";
            var result = await GetAsync<List<GetListCongThuc>>(url);
            return result;
        }

        public async Task<List<GetListDVTSanPham>> GetListCongThucs(int id)
        {
            var url = $"/api/CongThuc/donvitinh/{id}";
            var result = await GetAsync<List<GetListDVTSanPham>>(url);
            return result;
        }

        public async Task<List<GetListDVTSanPham>> GetListDonViTinhs(int id)
        {
            var url = $"/api/CongThuc/donvitinh/{id}";
            var result = await GetAsync<List<GetListDVTSanPham>>(url);
            return result;
        }

        public async Task<ApiResult<GetListIdCongThuc>> GetListIdCongThuc(int id)
        {
            var url = $"/api/CongThuc/congthuc/{id}";
            var result = await GetAsync<ApiResult<GetListIdCongThuc>>(url);
            return result;
        }

        public async Task<List<GetListNguyenVatLieu>> GetListNguyenVatLieu(ListBundleNguyenVatLieu bundle)
        {
            var url = $"/api/CongThuc/nguyenvatlieu?nhomLoaiNVL={bundle.NhomLoaiNVL}"
                                                + $"&loaiNguyenVatLieu={bundle.LoaiNguyenVatLieu}"
                                                + $"&tuKhoaNVL={bundle.TuKhoaNVL}";
            var result = await GetAsync<List<GetListNguyenVatLieu>>(url);
            return result;
        }

        public async Task<List<GetLoaiNguyenVatLieu>> GetLoaiNguyenVatLieu(NhomLoaiNVL? group)
        {
            var url = $"/api/CongThuc/loai-nguyenvatlieu?group=" + $"{group}";
            var result = await GetAsync<List<GetLoaiNguyenVatLieu>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iNguyenVatLieu(int idCongThuc, int idNguyenVatLieu)
        {
            var url = $"/api/CongThuc/kiemtra-nguyenvatlieu?idCongThuc=" + $"{idCongThuc}&idNguyenVatLieu={idNguyenVatLieu}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int idSP, int? id)
        {
            var url = $"/api/CongThuc/kiemtra-ten?ten=" + $"{ten}&idSP={idSP}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<GetTaoCongThuc>> Update(CapNhatCongThuc bundle)
        {
            var url = $"/api/CongThuc";
            var result = await Update<GetTaoCongThuc, CapNhatCongThuc>(url, bundle);
            return result;
        }

        public async Task<ApiResult<string>> UpdateDinhDauCongThuc(CapNhatDinhDauCongThuc bundle)
        {
            var url = $"/api/CongThuc/dinh-dau";
            var result = await Update<string, CapNhatDinhDauCongThuc>(url, bundle);
            return result;
        }
    }
}