using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.ViewModels.LoaiSanPham;
using QuanLyKho.ViewModels.NhomSanPham;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.KeHoachCheBienApiClient
{
    public interface IKeHoachCheBienApiClient
    {
        Task<ApiResult<NhanKetQuaCheBien>> Create(TaoKeHoachCheBien bundle);

        Task<ApiResult<ThongBaoModel>> Delete(long id);

        Task<ApiResult<bool>> DeleteCongThuc(long id);

        Task<ApiResult<NhanKetQuaCheBien>> Update(CapNhatKeHoachCheBien bundle);

        Task<List<NhanKetQuaCheBien>> GetAllKeHoachTheoThang(int thang, int nam);

        Task<List<CongThucSanPham>> GetCongThucSanPham(string tuKhoa, int idLoaiSanPham);

        Task<List<GetTenLoaiSP>> GetLoaiSanPhamKH(int id);

        Task<List<GetTenNhomSP>> GetNhomSanPhamKH();

        Task<List<DonViTinhModel>> GetDonViTinhKHCB(int idSanPham);

        Task<List<DonViTinhModel>> GetShowDonViTinhKHCB(int idSanPham);

        Task<List<NhanVienKHCheBien>> NhanVienKHCheBien(string tuKhoa);

        Task<ApiResult<KeHoachCheBienModel>> GetInfoKeHoachById(long id);

        Task<ApiResult<PagedResult<KeHoachCheBienModel>>> GetAllKeHoach(GetAllKeHoachCheBien bundle);

        Task<ApiResult<PagedResult<NhanKeHoachCheBien>>> NhanKeHoachCheBien(GetAllKeHoachCheBien bundle, string userName);

        Task<ApiResult<bool>> UpdateNhanKeHoach(long id);
    }
}