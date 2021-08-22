using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.LoaiNguyenVatLieuApiClient
{
    public class LoaiNguyenVatLieuApiClient : BaseApiClient, ILoaiNguyenVatLieuApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoaiNguyenVatLieuApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<LoaiNguyenVatLieuModel>> Create(TaoLoaiNguyenVatLieu bundle)
        {
            var url = $"/api/LoaiNguyenVatLieu";
            var result = await Create<LoaiNguyenVatLieuModel, TaoLoaiNguyenVatLieu>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var result = await Delete<bool>($"/api/LoaiNguyenVatLieu/" + id);
            return result;
        }

        public async Task<ApiResult<LoaiNguyenVatLieuModel>> GetById(int id)
        {
            var url = $"/api/LoaiNguyenVatLieu/" + $"{id}";
            var result = await GetAsync<ApiResult<LoaiNguyenVatLieuModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<LoaiNguyenVatLieuModel>>> GetLoaiNguyenVatLieuPaging(GetLoaiNguyenVatLieu bundle)
        {
            var url = $"/api/LoaiNguyenVatLieu/paging?pageIndex=" +
                                                    $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}" +
                                                    $"&nhomLoai={ bundle.NhomLoai}";
            var result = await GetAsync<ApiResult<PagedResult<LoaiNguyenVatLieuModel>>>(url);
            return result;
        }

        public async Task<List<GetTenLoaiNVL>> GetTenLoaiNVL()
        {
            var url = $"/api/LoaiNguyenVatLieu/get-all";
            var result = await GetAsync<List<GetTenLoaiNVL>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            var url = $"/api/LoaiNguyenVatLieu/kiemtra-ten?ten=" + $"{ten}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Update(CapNhatLoaiNguyenVatLieu bundle)
        {
            var url = $"/api/LoaiNguyenVatLieu";
            var result = await Update<bool, CapNhatLoaiNguyenVatLieu>(url, bundle);
            return result;
        }
    }
}