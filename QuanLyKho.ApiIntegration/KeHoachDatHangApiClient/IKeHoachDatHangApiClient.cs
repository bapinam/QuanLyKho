using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.ViewModels.KeHoachDatHang;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.KeHoachDatHangApiClient
{
    public interface IKeHoachDatHangApiClient
    {
        public Task<List<KeHoachCheBienModel>> KeHoachCheBienDuKien();

        public Task<ApiResult<KeHoachCheBienModel>> GetInfoKeHoachCheBienDuKien(long id);

        public Task<List<GetTenLoaiNVL>> GetLoaiNVLKH(int nhomNVL);

        public Task<List<NguyenVatLieuKH>> GetNguyenVatLieuKH(int idLoaiNVL, string tuKhoa);

        Task<List<DonViTinhModel>> GetDonViTinhKHDH(int idNVL);

        Task<List<DonViTinhModel>> GetShowDonViTinhKHDH(int idNVL);

        Task<List<NhanVienKHDatHang>> NhanVienKHDatHang(string tuKhoa);

        Task<ApiResult<NhanKetQuaDatHang>> Create(TaoKeHoachDatHang bundle);

        Task<List<NhanKetQuaDatHang>> GetAllKeHoachTheoThang(int thang, int nam);

        Task<ApiResult<KeHoachDatHangModel>> GetInfoKeHoachById(long id);

        Task<ApiResult<ThongBaoModel>> Delete(long id);

        Task<ApiResult<bool>> DeleteNguyenVatLieu(int id);

        Task<ApiResult<NhanKetQuaDatHang>> Update(CapNhatKeHoachDatHang bundle);

        Task<ApiResult<PagedResult<KeHoachDatHangModel>>> GetAllKeHoach(GetAllKeHoachDatHang bundle);

        Task<ApiResult<PagedResult<NhanKeHoachDatHang>>> NhanKeHoachDatHang(GetAllKeHoachDatHang bundle, string userName);

        Task<ApiResult<bool>> UpdateNhanKeHoach(long id);
    }
}