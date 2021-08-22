using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DatHang;
using QuanLyKho.ViewModels.KeHoachDatHang;
using QuanLyKho.ViewModels.NhaCungCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.DatHangService
{
    public interface IDatHangService
    {
        Task<List<KeHoachDatHangModel>> GetAllKeHoachDatHangChuaHoanThanh(TimKiemDatHang bundle);

        Task<List<DanhSachNguyenLieuDatHang>> GetDanhSachDatHang(long id);

        Task<List<NhaCungCapModel>> NhaCungCapDatHang(string tuKhoa);

        Task<ApiResult<PhieuNhapModel>> Create(NhapHoaDon bundle);

        Task<ApiResult<string>> HuyChiTietDatHang(long id);

        Task<List<PhieuNhapModel>> GetAllPhieuNhapChuaTra(TimKiemDatHang bundle);

        Task<ApiResult<PagedResult<PhieuNhapModel>>> GetAllPhieuNhapDaTra(TimKiemPhieuNhapDaTra bundle);

        Task<ApiResult<ChiTietPhieuNhapModel>> GetChiTietPhieuNhap(long id);

        Task<ApiResult<PhieuNhapModel>> UpdateThanhToan(long id, string tienDaTra);

        Task<ApiResult<TienDaThanhToan>> GetTienDaThanhToan(long id);
    }
}