using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.ThongBaoApiClient
{
    public class ThongBaoApiClient : BaseApiClient, IThongBaoApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public ThongBaoApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<bool>> Delete(long id)
        {
            var result = await Delete<bool>($"/api/ThongBao/" + id);
            return result;
        }

        public async Task<ApiResult<string>> DeleteAll()
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            var response = await client.DeleteAsync("/api/ThongBao/delete-all");

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PagedResult<ThongBaoModel>>> GetThongBaoPaging(SetThongBao bundle)
        {
            var url = $"/api/ThongBao/paging?pageIndex="
                              + $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}"
                              + $"&ngayNhanThongBao={bundle.NgayNhanThongBao}&trangThaiXem={bundle.TrangThaiXem}"
                              + $"&userName={bundle.UserName}";
            var result = await GetAsync<ApiResult<PagedResult<ThongBaoModel>>>(url);
            return result;
        }

        public async Task<List<ThongBaoModel>> GetTopThongBao(string userName)
        {
            var url = $"/api/ThongBao/get-top/{userName}";
            var result = await GetAsync<List<ThongBaoModel>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateDaXem(long id)
        {
            var url = $"/api/ThongBao/{id}";
            var result = await Update<bool, long>(url, id);
            return result;
        }
    }
}