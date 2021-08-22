using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.SanPhamApiClient
{
    public class SanPhamApiClient : BaseApiClient, ISanPhamApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public SanPhamApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<SanPhamModel>> Create(TaoToanBoSanPham bundle)
        {
            var url = $"/api/SanPham";
            var nguyenVatLieu = new TaoSanPham()
            {
                MaSo = bundle.MaSo,
                Ten = bundle.Ten,
                MoTa = bundle.MoTa,
                NhacNho = bundle.NhacNho,
                MucTonCaoNhat = bundle.MucTonCaoNhat != null ? bundle.MucTonCaoNhat : null,
                MucTonThapNhat = bundle.MucTonThapNhat != null ? bundle.MucTonThapNhat : 0,
                TenDVTCoBan = bundle.TenDVTCoBan,
                TenDVT = bundle.TenDVT,
                GiaTriDVT = bundle.GiaTriDVT != null ? bundle.GiaTriDVT : null,
                IdLoaiSanPham = bundle.IdLoaiSanPham
            };
            var result = await Create<SanPhamModel, TaoSanPham>(url, nguyenVatLieu);

            if (bundle.HinhAnh != null)
            {
                var hinhAnh = await this.CapNhatHinhAnh(result.ResultObj.Id, bundle.HinhAnh);
                result.ResultObj.HinhAnh = hinhAnh.ResultObj;
            }

            return result;
        }

        private async Task<ApiResult<string>> CapNhatHinhAnh(int id, IFormFile hinhAnh)
        {
            var requestContent = new MultipartFormDataContent();

            byte[] data;
            using (var br = new BinaryReader(hinhAnh.OpenReadStream()))
            {
                data = br.ReadBytes((int)hinhAnh.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);
            requestContent.Add(bytes, "hinhAnh", hinhAnh.FileName);

            var url = $"/api/SanPham/hinh-anh/{id}";
            var result = await UpdateHinhAnh<string>(url, requestContent);
            return result;
        }

        public async Task<ApiResult<PagedResult<SanPhamModel>>> GetSanPhamPaging(GetSanPhamPaging bundle)
        {
            var url = $"/api/SanPham/paging?pageIndex="
                                  + $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}"
                                  + $"&idLoaiSanPham={ bundle.IdLoaiSanPham}";
            var result = await GetAsync<ApiResult<PagedResult<SanPhamModel>>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var url = $"/api/SanPham/{id}";
            var result = await Delete<bool>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            var url = $"/api/SanPham/kiemtra-ten?ten=" + $"{ten}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<SanPhamModel>> Update(CapNhatToanBoSanPham bundle)
        {
            var url = $"/api/SanPham";
            var nguyenVatLieu = new CapNhatSanPham()
            {
                Ten = bundle.Ten,
                MoTa = bundle.MoTa,
                IdLoaiSanPham = bundle.IdLoaiSanPham,
                Id = bundle.IdSanPham
            };
            var result = await Update<SanPhamModel, CapNhatSanPham>(url, nguyenVatLieu);

            if (bundle.HinhAnh != null)
            {
                var hinhAnh = await this.CapNhatHinhAnh(result.ResultObj.Id, bundle.HinhAnh);
                result.ResultObj.HinhAnh = hinhAnh.ResultObj;
            }

            return result;
        }

        public async Task<ApiResult<SanPhamModel>> GetByUpdateSanPham(int id)
        {
            var url = $"/api/SanPham/cap-nhat/{id}";
            var result = await GetAsync<ApiResult<SanPhamModel>>(url);
            return result;
        }

        public async Task<ApiResult<SanPhamShowAll>> GetShowAllSanPham(int id)
        {
            var url = $"/api/SanPham/xem/{id}";
            var result = await GetAsync<ApiResult<SanPhamShowAll>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateNhacNho(NhacNhoSPModel bundle)
        {
            var url = $"/api/SanPham/nhac-nho";
            var result = await Update<bool, NhacNhoSPModel>(url, bundle);
            return result;
        }

        public async Task<ApiResult<NhacNhoSPModel>> GetNhacNho(int id)
        {
            var url = $"/api/SanPham/nhac-nho/{id}";
            var result = await GetAsync<ApiResult<NhacNhoSPModel>>(url);
            return result;
        }
    }
}