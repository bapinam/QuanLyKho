using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NguyenVatLieu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.NguyenVatLieuApiClient
{
    public class NguyenVatLieuApiClient : BaseApiClient, INguyenVatLieuApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public NguyenVatLieuApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<NguyenVatLieuModel>> Create(TaoToanBoNguyenVatLieu bundle)
        {
            var url = $"/api/NguyenVatLieu";
            var nguyenVatLieu = new TaoNguyenVatLieu()
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
                IdLoaiNguyenVatLieu = bundle.IdLoaiNguyenVatLieu
            };
            var result = await Create<NguyenVatLieuModel, TaoNguyenVatLieu>(url, nguyenVatLieu);

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

            var url = $"/api/NguyenVatLieu/hinh-anh/{id}";
            var result = await UpdateHinhAnh<string>(url, requestContent);
            return result;
        }

        public async Task<ApiResult<PagedResult<NguyenVatLieuModel>>> GetNguyenVatLieuPaging(GetNguyenVatLieuPaging bundle)
        {
            var url = $"/api/NguyenVatLieu/paging?pageIndex="
                                  + $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}"
                                  + $"&idLoaiNguyenVatLieu={ bundle.IdLoaiNguyenVatLieu}";
            var result = await GetAsync<ApiResult<PagedResult<NguyenVatLieuModel>>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var url = $"/api/NguyenVatLieu/{id}";
            var result = await Delete<bool>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            var url = $"/api/NguyenVatLieu/kiemtra-ten?ten=" + $"{ten}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<NguyenVatLieuModel>> Update(CapNhatToanBoNguyenVatLieu bundle)
        {
            var url = $"/api/NguyenVatLieu";
            var nguyenVatLieu = new CapNhatNguyenVatLieu()
            {
                Ten = bundle.Ten,
                MoTa = bundle.MoTa,
                IdLoaiNguyenVatLieu = bundle.IdLoaiNguyenVatLieu,
                Id = bundle.IdNguyenVatLieu
            };
            var result = await Update<NguyenVatLieuModel, CapNhatNguyenVatLieu>(url, nguyenVatLieu);

            if (bundle.HinhAnh != null)
            {
                var hinhAnh = await this.CapNhatHinhAnh(result.ResultObj.Id, bundle.HinhAnh);
                result.ResultObj.HinhAnh = hinhAnh.ResultObj;
            }

            return result;
        }

        public async Task<ApiResult<NguyenVatLieuModel>> GetByUpdateNguyenVatLieu(int id)
        {
            var url = $"/api/NguyenVatLieu/cap-nhat/{id}";
            var result = await GetAsync<ApiResult<NguyenVatLieuModel>>(url);
            return result;
        }

        public async Task<ApiResult<NguyenVatLieuShowAll>> GetShowAllNguyenVatLieu(int id)
        {
            var url = $"/api/NguyenVatLieu/xem/{id}";
            var result = await GetAsync<ApiResult<NguyenVatLieuShowAll>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateNhacNho(NhacNhoModel bundle)
        {
            var url = $"/api/NguyenVatLieu/nhac-nho";
            var result = await Update<bool, NhacNhoModel>(url, bundle);
            return result;
        }

        public async Task<ApiResult<NhacNhoModel>> GetNhacNho(int id)
        {
            var url = $"/api/NguyenVatLieu/nhac-nho/{id}";
            var result = await GetAsync<ApiResult<NhacNhoModel>>(url);
            return result;
        }
    }
}