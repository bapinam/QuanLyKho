using QuanLyKho.ViewModels.CheBien;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.KeHoachCheBien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.CheBienService
{
    public interface ICheBienService
    {
        Task<List<KeHoachCheBienModel>> GetAllKeHoachCheBienChuaHoanThanh(TimKiemCheBien bundle);

        Task<List<CongThucModelKeHoach>> GetCongThucKeHoach(long id);

        Task<ApiResult<NhanKetQuaThongBaoCB>> Create(TaoPhieuCheBien bundle);

        Task<List<PhieuCheBienModel>> GetPhieuCheBienDangCho(TimKiemCheBien bundle);

        Task<ApiResult<ChiTietPhieuCheBienModel>> GetIdInforCheBien(long id);

        Task<ApiResult<NhanKetQuaThongBaoCB>> HuyPhieuCheBien(long id, string nguoiDuyet);

        Task<ApiResult<NhanKetQuaThongBaoCB>> CapNhatTrangThaiCheBien(long id, string nguoiDuyet);

        Task<ApiResult<PagedResult<PhieuCheBienModel>>> GetAllPhieuCheBien(TimKiemPhieuCheBien bundle);

        Task<List<DanhSachCheBien>> GetAllDanhSachChebien(long id);

        Task<ApiResult<bool>> HuyChiTietCheBien(long id);
    }
}