using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.QuanLyMaSo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.QuanLyMaSoApiClient
{
    public class QuanLyMaSoApiClient : BaseApiClient, IQuanLyMaSoApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public QuanLyMaSoApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<bool>> Create(TaoMaSo bundle)
        {
            var url = $"/api/QuanLyMaSo";
            var result = await Create<bool, TaoMaSo>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var result = await Delete<bool>($"/api/QuanLyMaSo/" + id);
            return result;
        }

        public async Task<List<LoaiMaSo>> GetAll(MaLoai loai)
        {
            var url = $"/api/QuanLyMaSo/get-all?loai={loai}";
            var result = await GetAsync<List<LoaiMaSo>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            var url = $"/api/QuanLyMaSo/kiemtra-ten?ten=" + $"{ten}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Update(CapNhatMaSo bundle)
        {
            var url = $"/api/QuanLyMaSo";
            var result = await Update<bool, CapNhatMaSo>(url, bundle);
            return result;
        }
    }
}