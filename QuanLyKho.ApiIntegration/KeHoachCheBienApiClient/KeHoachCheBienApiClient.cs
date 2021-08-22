using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.ViewModels.LoaiSanPham;
using QuanLyKho.ViewModels.NhomSanPham;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.KeHoachCheBienApiClient
{
    public class KeHoachCheBienApiClient : BaseApiClient, IKeHoachCheBienApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public KeHoachCheBienApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<NhanKetQuaCheBien>> Create(TaoKeHoachCheBien bundle)
        {
            var url = $"/api/KeHoachCheBien";
            var result = await Create<NhanKetQuaCheBien, TaoKeHoachCheBien>(url, bundle);
            return result;
        }

        public async Task<ApiResult<ThongBaoModel>> Delete(long id)
        {
            var result = await Delete<ThongBaoModel>($"/api/KeHoachCheBien/" + id);
            return result;
        }

        public async Task<ApiResult<bool>> DeleteCongThuc(long id)
        {
            var result = await Delete<bool>($"/api/KeHoachCheBien/cong-thuc/" + id);
            return result;
        }

        public async Task<ApiResult<PagedResult<KeHoachCheBienModel>>> GetAllKeHoach(GetAllKeHoachCheBien bundle)
        {
            var url = $"/api/KeHoachCheBien/paging?pageIndex=" +
                             $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}&ngayDuKien={bundle.NgayDuKien}"
                             + $"&trangThai={bundle.TrangThai}";
            var result = await GetAsync<ApiResult<PagedResult<KeHoachCheBienModel>>>(url);
            return result;
        }

        public async Task<List<NhanKetQuaCheBien>> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var url = $"/api/KeHoachCheBien/theo-thang?thang=" + $"{thang}&nam={nam}";
            var result = await GetAsync<List<NhanKetQuaCheBien>>(url);
            return result;
        }

        public async Task<List<CongThucSanPham>> GetCongThucSanPham(string tuKhoa, int idLoaiSanPham)
        {
            var url = $"/api/KeHoachCheBien/congthuc-sanpham?tuKhoa={tuKhoa}&idLoaiSanPham={idLoaiSanPham}";
            var result = await GetAsync<List<CongThucSanPham>>(url);
            return result;
        }

        public async Task<List<DonViTinhModel>> GetDonViTinhKHCB(int idSanPham)
        {
            var url = $"/api/KeHoachCheBien/donvitinh/{idSanPham}";
            var result = await GetAsync<List<DonViTinhModel>>(url);
            return result;
        }

        public async Task<ApiResult<KeHoachCheBienModel>> GetInfoKeHoachById(long id)
        {
            var url = $"/api/KeHoachCheBien/xem-kehoach/{id}";
            var result = await GetAsync<ApiResult<KeHoachCheBienModel>>(url);
            return result;
        }

        public async Task<List<GetTenLoaiSP>> GetLoaiSanPhamKH(int id)
        {
            var url = $"/api/KeHoachCheBien/loai-sanpham/{id}";
            var result = await GetAsync<List<GetTenLoaiSP>>(url);
            return result;
        }

        public async Task<List<GetTenNhomSP>> GetNhomSanPhamKH()
        {
            var url = $"/api/KeHoachCheBien/nhom-sanpham";
            var result = await GetAsync<List<GetTenNhomSP>>(url);
            return result;
        }

        public async Task<List<DonViTinhModel>> GetShowDonViTinhKHCB(int idSanPham)
        {
            var url = $"/api/KeHoachCheBien/xem-donvitinh/{idSanPham}";
            var result = await GetAsync<List<DonViTinhModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<NhanKeHoachCheBien>>> NhanKeHoachCheBien(GetAllKeHoachCheBien bundle, string userName)
        {
            var url = $"/api/KeHoachCheBien/paging/nhan-kehoach?pageIndex=" +
                            $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}&ngayDuKien={bundle.NgayDuKien}"
                            + $"&trangThai={bundle.TrangThai}&userName={userName}";
            var result = await GetAsync<ApiResult<PagedResult<NhanKeHoachCheBien>>>(url);
            return result;
        }

        public async Task<List<NhanVienKHCheBien>> NhanVienKHCheBien(string tuKhoa)
        {
            var url = $"/api/KeHoachCheBien/nhan-vien?tuKhoa={tuKhoa}";
            var result = await GetAsync<List<NhanVienKHCheBien>>(url);
            return result;
        }

        public async Task<ApiResult<NhanKetQuaCheBien>> Update(CapNhatKeHoachCheBien bundle)
        {
            var url = $"/api/KeHoachCheBien";
            var result = await Update<NhanKetQuaCheBien, CapNhatKeHoachCheBien>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateNhanKeHoach(long id)
        {
            var url = $"/api/KeHoachCheBien/nhan-kehoach/{id}";
            var result = await Update<bool, long>(url, id);
            return result;
        }
    }
}