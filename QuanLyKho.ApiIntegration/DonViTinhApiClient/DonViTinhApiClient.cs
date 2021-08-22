using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.DonViTinhApiClient
{
    public class DonViTinhApiClient : BaseApiClient, IDonViTinhApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public DonViTinhApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<DonViTinhModel>> Create(TaoDonViTinh bundle)
        {
            var url = $"/api/DonViTinh";
            var result = await Create<DonViTinhModel, TaoDonViTinh>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(long id)
        {
            var result = await Delete<bool>($"/api/DonViTinh/" + id);
            return result;
        }

        public async Task<List<DonViTinhModel>> GetDonViTinh(int id, LoaiDongGoi loaiDongGoi)
        {
            var url = $"/api/DonViTinh/" + $"{id}/{loaiDongGoi}";
            var result = await GetAsync<List<DonViTinhModel>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int id, long? idDVT, bool loaiDongGoi)
        {
            var url = $"/api/DonViTinh/kiemtra-ten?ten=" + $"{ten}&id={id}&idDVT={idDVT}&loaiDongGoi={loaiDongGoi}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Update(CapNhatDonViTinh bundle)
        {
            var url = $"/api/DonViTinh";
            var result = await Update<bool, CapNhatDonViTinh>(url, bundle);
            return result;
        }
    }
}