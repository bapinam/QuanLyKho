using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.LoaiSanPhamApiClient
{
    public class LoaiSanPhamApiClient : BaseApiClient, ILoaiSanPhamApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoaiSanPhamApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<LoaiSanPhamModel>> Create(TaoLoaiSanPham bundle)
        {
            var url = $"/api/LoaiSanPham";
            var result = await Create<LoaiSanPhamModel, TaoLoaiSanPham>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var result = await Delete<bool>($"/api/LoaiSanPham/" + id);
            return result;
        }

        public async Task<ApiResult<LoaiSanPhamModel>> GetById(int id)
        {
            var url = $"/api/LoaiSanPham/" + $"{id}";
            var result = await GetAsync<ApiResult<LoaiSanPhamModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<LoaiSanPhamModel>>> GetLoaiSanPhamPaging(GetLoaiSanPham bundle)
        {
            var url = $"/api/LoaiSanPham/paging?pageIndex=" +
                                                    $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}" +
                                                    $"&idNhomLoai={ bundle.IdNhomLoai}";
            var result = await GetAsync<ApiResult<PagedResult<LoaiSanPhamModel>>>(url);
            return result;
        }

        public async Task<List<GetTenLoaiSP>> GetTenLoaiSP()
        {
            var url = $"/api/LoaiSanPham/get-all";
            var result = await GetAsync<List<GetTenLoaiSP>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            var url = $"/api/LoaiSanPham/kiemtra-ten?ten=" + $"{ten}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Update(CapNhatLoaiSanPham bundle)
        {
            var url = $"/api/LoaiSanPham";
            var result = await Update<bool, CapNhatLoaiSanPham>(url, bundle);
            return result;
        }
    }
}