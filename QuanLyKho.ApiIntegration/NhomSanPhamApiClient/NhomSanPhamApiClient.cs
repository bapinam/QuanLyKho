using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NhomSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.NhomSanPhamApiClient
{
    public class NhomSanPhamApiClient : BaseApiClient, INhomSanPhamApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public NhomSanPhamApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<NhomSanPhamModel>> Create(TaoNhomSanPham bundle)
        {
            var url = $"/api/NhomSanPham";
            var result = await Create<NhomSanPhamModel, TaoNhomSanPham>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var result = await Delete<bool>($"/api/NhomSanPham/" + id);
            return result;
        }

        public async Task<ApiResult<NhomSanPhamModel>> GetById(int id)
        {
            var url = $"/api/NhomSanPham/" + $"{id}";
            var result = await GetAsync<ApiResult<NhomSanPhamModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<NhomSanPhamModel>>> GetNhomSanPhamPaging(GetNhomSanPham bundle)
        {
            var url = $"/api/NhomSanPham/paging?pageIndex=" +
                                                    $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}"; var result = await GetAsync<ApiResult<PagedResult<NhomSanPhamModel>>>(url);
            return result;
        }

        public async Task<List<GetTenNhomSP>> GetTenNhomSP()
        {
            var url = $"/api/NhomSanPham/get-all";
            var result = await GetAsync<List<GetTenNhomSP>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            var url = $"/api/NhomSanPham/kiemtra-ten?ten=" + $"{ten}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Update(CapNhatNhomSanPham bundle)
        {
            var url = $"/api/NhomSanPham";
            var result = await Update<bool, CapNhatNhomSanPham>(url, bundle);
            return result;
        }
    }
}