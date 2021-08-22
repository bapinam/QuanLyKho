using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.CheBien;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.KeHoachCheBien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.CheBienApiClient
{
    public class CheBienApiClient : BaseApiClient, ICheBienApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public CheBienApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<NhanKetQuaThongBaoCB>> CapNhatTrangThaiCheBien(long id, string userName)
        {
            var url = $"/api/CheBien/cap-nhat/{id}/{userName}";
            var result = await GetAsync<ApiResult<NhanKetQuaThongBaoCB>>(url);
            return result;
        }

        public async Task<ApiResult<NhanKetQuaThongBaoCB>> Create(TaoPhieuCheBien bundle)
        {
            var url = $"/api/CheBien";
            var result = await Create<NhanKetQuaThongBaoCB, TaoPhieuCheBien>(url, bundle);
            return result;
        }

        public async Task<List<DanhSachCheBien>> GetAllDanhSachChebien(long id)
        {
            var url = $"/api/CheBien/danh-sach-chebien/{id}";
            var result = await GetAsync<List<DanhSachCheBien>>(url);
            return result;
        }

        public async Task<List<KeHoachCheBienModel>> GetAllKeHoachCheBienChuaHoanThanh(TimKiemCheBien bundle)
        {
            var url = $"/api/CheBien/ke-hoach?tuKhoa={bundle.TuKhoa}&ngay={bundle.Ngay}";
            var result = await GetAsync<List<KeHoachCheBienModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<PhieuCheBienModel>>> GetAllPhieuCheBien(TimKiemPhieuCheBien bundle)
        {
            var url = $"/api/CheBien/paging?pageIndex=" +
                            $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}&ngay={bundle.Ngay}"
                            + $"&trangThai={bundle.TrangThai}";
            var result = await GetAsync<ApiResult<PagedResult<PhieuCheBienModel>>>(url);
            return result;
        }

        public async Task<List<CongThucModelKeHoach>> GetCongThucKeHoach(long id)
        {
            var url = $"/api/CheBien/cong-thuc/{id}";
            var result = await GetAsync<List<CongThucModelKeHoach>>(url);
            return result;
        }

        public async Task<ApiResult<ChiTietPhieuCheBienModel>> GetIdInforCheBien(long id)
        {
            var url = $"/api/CheBien/chi-tiet-phieu-chebien/{id}";
            var result = await GetAsync<ApiResult<ChiTietPhieuCheBienModel>>(url);
            return result;
        }

        public async Task<List<PhieuCheBienModel>> GetPhieuCheBienDangCho(TimKiemCheBien bundle)
        {
            var url = $"/api/CheBien/phieu-chebien?tuKhoa={bundle.TuKhoa}&ngay={bundle.Ngay}";
            var result = await GetAsync<List<PhieuCheBienModel>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> HuyChiTietCheBien(long id)
        {
            var url = $"/api/CheBien/huy-chebien/{id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<NhanKetQuaThongBaoCB>> HuyPhieuCheBien(long id, string nguoiDuyet)
        {
            var url = $"/api/CheBien/huy/{id}/{nguoiDuyet}";
            var result = await GetAsync<ApiResult<NhanKetQuaThongBaoCB>>(url);
            return result;
        }
    }
}