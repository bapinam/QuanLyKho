using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NhaCungCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.NhaCungCapApiClient
{
    public class NhaCungCapApiClient : BaseApiClient, INhaCungCapApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public NhaCungCapApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<NhaCungCapModel>> Create(TaoNhaCungCap bundle)
        {
            var url = $"/api/NhaCungCap";
            var result = await Create<NhaCungCapModel, TaoNhaCungCap>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var result = await Delete<bool>($"/api/NhaCungCap/" + id);
            return result;
        }

        public async Task<ApiResult<NhaCungCapModel>> GetById(int id)
        {
            var url = $"/api/NhaCungCap/" + $"{id}";
            var result = await GetAsync<ApiResult<NhaCungCapModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<NhaCungCapModel>>> GetNhaCungCapPaging(GetNhaCungCapPagingRequest bundle)
        {
            var url = $"/api/NhaCungCap/paging?pageIndex=" +
                                         $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}";
            var result = await GetAsync<ApiResult<PagedResult<NhaCungCapModel>>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iEmail(string email, int? id)
        {
            var url = $"/api/NhaCungCap/kiemtra-email?email=" + $"{email}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iMaThue(string thue, int? id)
        {
            var url = $"/api/NhaCungCap/kiemtra-sothue?thue=" + $"{thue}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Update(CapNhatNhaCungCap bundle)
        {
            var url = $"/api/NhaCungCap";
            var result = await Update<bool, CapNhatNhaCungCap>(url, bundle);
            return result;
        }
    }
}