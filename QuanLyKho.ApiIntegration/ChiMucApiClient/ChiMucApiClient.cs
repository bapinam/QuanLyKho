using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.ChiMuc;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.User;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.ChiMucApiClient
{
    public class ChiMucApiClient : BaseApiClient, IChiMucApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public ChiMucApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<ModelChiMucGetOne>> Create(TaoChiMuc bundle)
        {
            var url = $"/api/ChiMuc";
            var result = await Create<ModelChiMucGetOne, TaoChiMuc>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var result = await Delete<bool>($"/api/ChiMuc/" + id);
            return result;
        }

        public async Task<ApiResult<UpdateChiMucModel>> DeleteNgayNghi(int id)
        {
            var result = await Delete<UpdateChiMucModel>($"/api/ChiMuc/ngay-nghi/" + id);
            return result;
        }

        public async Task<ApiResult<PagedResult<ModelChimuc>>> GetAll(GetAllChiMuc bundle)
        {
            var url = $"/api/ChiMuc/paging?pageIndex=" +
                             $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}";
            var result = await GetAsync<ApiResult<PagedResult<ModelChimuc>>>(url);
            return result;
        }

        public async Task<ApiResult<ChiMucTheoId>> GetChiMucTheoId(int id)
        {
            var url = $"/api/ChiMuc/" + $"{id}";
            var result = await GetAsync<ApiResult<ChiMucTheoId>>(url);
            return result;
        }

        public async Task<ApiResult<string>> GetName(string ten)
        {
            var url = $"/api/ChiMuc/ten/" + $"{ten}";
            var result = await GetAsync<ApiResult<string>>(url);
            return result;
        }

        public async Task<ApiResult<int>> iNgayLamViec()
        {
            var url = $"/api/ChiMuc";
            var result = await GetAsync<ApiResult<int>>(url);
            return result;
        }

        public async Task<ApiResult<UpdateChiMucModel>> Update(CapNhatChiMuc bundle)
        {
            var url = $"/api/ChiMuc";
            var result = await Update<UpdateChiMucModel, CapNhatChiMuc>(url, bundle);
            return result;
        }
    }
}