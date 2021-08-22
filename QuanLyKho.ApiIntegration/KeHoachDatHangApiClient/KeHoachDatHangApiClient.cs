using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.ViewModels.KeHoachDatHang;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.KeHoachDatHangApiClient
{
    public class KeHoachDatHangApiClient : BaseApiClient, IKeHoachDatHangApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public KeHoachDatHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<PagedResult<KeHoachDatHangModel>>> GetAllKeHoach(GetAllKeHoachDatHang bundle)
        {
            var url = $"/api/KeHoachDatHang/paging?pageIndex=" +
                             $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}&ngayDuKien={bundle.NgayDuKien}"
                             + $"&trangThai={bundle.TrangThai}";
            var result = await GetAsync<ApiResult<PagedResult<KeHoachDatHangModel>>>(url);
            return result;
        }

        public async Task<ApiResult<KeHoachCheBienModel>> GetInfoKeHoachCheBienDuKien(long id)
        {
            var url = $"/api/KeHoachDatHang/du-kien/{id}";
            var result = await GetAsync<ApiResult<KeHoachCheBienModel>>(url);
            return result;
        }

        public async Task<List<GetTenLoaiNVL>> GetLoaiNVLKH(int nhomNVL)
        {
            var url = $"/api/KeHoachDatHang/loai-nguyenvatlieu/{nhomNVL}";
            var result = await GetAsync<List<GetTenLoaiNVL>>(url);
            return result;
        }

        public async Task<List<NguyenVatLieuKH>> GetNguyenVatLieuKH(int idLoaiNVL, string tuKhoa)
        {
            var url = $"/api/KeHoachDatHang/nguyen-vat-lieu?idLoaiNVL={idLoaiNVL}&tuKhoa={tuKhoa}";
            var result = await GetAsync<List<NguyenVatLieuKH>>(url);
            return result;
        }

        public async Task<List<DonViTinhModel>> GetShowDonViTinhKHDH(int idNVL)
        {
            var url = $"/api/KeHoachDatHang/xem-donvitinh/{idNVL}";
            var result = await GetAsync<List<DonViTinhModel>>(url);
            return result;
        }

        public async Task<List<DonViTinhModel>> GetDonViTinhKHDH(int idNVL)
        {
            var url = $"/api/KeHoachDatHang/donvitinh/{idNVL}";
            var result = await GetAsync<List<DonViTinhModel>>(url);
            return result;
        }

        public async Task<List<KeHoachCheBienModel>> KeHoachCheBienDuKien()
        {
            var url = $"/api/KeHoachDatHang/kehoach-dukien";
            var result = await GetAsync<List<KeHoachCheBienModel>>(url);
            return result;
        }

        public async Task<List<NhanVienKHDatHang>> NhanVienKHDatHang(string tuKhoa)
        {
            var url = $"/api/KeHoachDatHang/nhan-vien?tuKhoa={tuKhoa}";
            var result = await GetAsync<List<NhanVienKHDatHang>>(url);
            return result;
        }

        public async Task<ApiResult<NhanKetQuaDatHang>> Create(TaoKeHoachDatHang bundle)
        {
            var url = $"/api/KeHoachDatHang";
            var result = await Create<NhanKetQuaDatHang, TaoKeHoachDatHang>(url, bundle);
            return result;
        }

        public async Task<List<NhanKetQuaDatHang>> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var url = $"/api/KeHoachDatHang/theo-thang?thang=" + $"{thang}&nam={nam}";
            var result = await GetAsync<List<NhanKetQuaDatHang>>(url);
            return result;
        }

        public async Task<ApiResult<KeHoachDatHangModel>> GetInfoKeHoachById(long id)
        {
            var url = $"/api/KeHoachDatHang/xem-kehoach/{id}";
            var result = await GetAsync<ApiResult<KeHoachDatHangModel>>(url);
            return result;
        }

        public async Task<ApiResult<ThongBaoModel>> Delete(long id)
        {
            var result = await Delete<ThongBaoModel>($"/api/KeHoachDatHang/" + id);
            return result;
        }

        public async Task<ApiResult<bool>> DeleteNguyenVatLieu(int id)
        {
            var result = await Delete<bool>($"/api/KeHoachDatHang/nguyen-vat-lieu/" + id);
            return result;
        }

        public async Task<ApiResult<NhanKetQuaDatHang>> Update(CapNhatKeHoachDatHang bundle)
        {
            var url = $"/api/KeHoachDatHang";
            var result = await Update<NhanKetQuaDatHang, CapNhatKeHoachDatHang>(url, bundle);
            return result;
        }

        public async Task<ApiResult<PagedResult<NhanKeHoachDatHang>>> NhanKeHoachDatHang(GetAllKeHoachDatHang bundle, string userName)
        {
            var url = $"/api/KeHoachDatHang/paging/nhan-kehoach?pageIndex=" +
                           $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}&ngayDuKien={bundle.NgayDuKien}"
                           + $"&trangThai={bundle.TrangThai}&userName={userName}";
            var result = await GetAsync<ApiResult<PagedResult<NhanKeHoachDatHang>>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateNhanKeHoach(long id)
        {
            var url = $"/api/KeHoachDatHang/nhan-kehoach/{id}";
            var result = await Update<bool, long>(url, id);
            return result;
        }
    }
}