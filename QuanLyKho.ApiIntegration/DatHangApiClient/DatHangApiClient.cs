using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DatHang;
using QuanLyKho.ViewModels.KeHoachDatHang;
using QuanLyKho.ViewModels.NhaCungCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.DatHangApiClient
{
    public class DatHangApiClient : BaseApiClient, IDatHangApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public DatHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<PhieuNhapModel>> Create(NhapHoaDon bundle)
        {
            var url = $"/api/DatHang";
            var result = await Create<PhieuNhapModel, NhapHoaDon>(url, bundle);
            return result;
        }

        public async Task<List<KeHoachDatHangModel>> GetAllKeHoachDatHangChuaHoanThanh(TimKiemDatHang bundle)
        {
            var url = $"/api/DatHang/ke-hoach?tuKhoa={bundle.TuKhoa}&ngay={bundle.Ngay}";
            var result = await GetAsync<List<KeHoachDatHangModel>>(url);
            return result;
        }

        public async Task<List<PhieuNhapModel>> GetAllPhieuNhapChuaTra(TimKiemDatHang bundle)
        {
            var url = $"/api/DatHang/phieu-chuatra?tuKhoa={bundle.TuKhoa}&ngay={bundle.Ngay}";
            var result = await GetAsync<List<PhieuNhapModel>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<PhieuNhapModel>>> GetAllPhieuNhapDaTra(TimKiemPhieuNhapDaTra bundle)
        {
            var url = $"/api/DatHang/paging?pageIndex=" +
                             $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}&ngay={bundle.Ngay}";
            var result = await GetAsync<ApiResult<PagedResult<PhieuNhapModel>>>(url);
            return result;
        }

        public async Task<ApiResult<ChiTietPhieuNhapModel>> GetChiTietPhieuNhap(long id)
        {
            var url = $"/api/DatHang/chi-tiet-phieunhap/{id}";
            var result = await GetAsync<ApiResult<ChiTietPhieuNhapModel>>(url);
            return result;
        }

        public async Task<List<DanhSachNguyenLieuDatHang>> GetDanhSachDatHang(long id)
        {
            var url = $"/api/DatHang/danh-sach-dathang/{id}";
            var result = await GetAsync<List<DanhSachNguyenLieuDatHang>>(url);
            return result;
        }

        public async Task<ApiResult<TienDaThanhToan>> GetTienDaThanhToan(long id)
        {
            var url = $"/api/DatHang/lay-tien-da-thanhtoan/{id}";
            var result = await GetAsync<ApiResult<TienDaThanhToan>>(url);
            return result;
        }

        public async Task<ApiResult<string>> HuyChiTietDatHang(long id)
        {
            var url = $"/api/DatHang/huy-chitiet-dathang/{id}";
            var result = await GetAsync<ApiResult<string>>(url);
            return result;
        }

        public async Task<List<NhaCungCapModel>> NhaCungCapDatHang(string tuKhoa)
        {
            var url = $"/api/DatHang/nha-cung-cap/{tuKhoa}";
            var result = await GetAsync<List<NhaCungCapModel>>(url);
            return result;
        }

        public async Task<ApiResult<PhieuNhapModel>> UpdateThanhToan(long id, string tienDaTra)
        {
            var url = $"/api/DatHang/cap-nhat-thanhtoan/{id}/{tienDaTra}";
            var result = await GetAsync<ApiResult<PhieuNhapModel>>(url);
            return result;
        }
    }
}