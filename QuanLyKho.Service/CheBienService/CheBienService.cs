using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.ViewModels.CheBien;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.CheBienService
{
    public class CheBienService : ICheBienService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IThongBaoService _thongBaoService;

        public CheBienService(QuanLyKhoDbContext context, UserManager<AppUser> userManager, IThongBaoService thongBaoService)
        {
            _context = context;
            _userManager = userManager;
            _thongBaoService = thongBaoService;
        }

        public async Task<ApiResult<NhanKetQuaThongBaoCB>> CapNhatTrangThaiCheBien(long id, string nguoiDuyet)
        {
            var phieu = await _context.PhieuCheBiens.Include(x => x.ChiTietPhieuCheBiens)
                .ThenInclude(x => x.CongThuc).ThenInclude(x => x.SanPham).ThenInclude(x => x.DonViTinhs)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
            if (phieu == null)
            {
                return new ApiErrorResult<NhanKetQuaThongBaoCB>("Không tồn tại id");
            }

            phieu.TrangThaiPhieu = TrangThaiPhieu.HoanThanh;

            var nguoi = await _userManager.FindByNameAsync(nguoiDuyet);
            phieu.IdNguoiDuyet = nguoi.Id;
            phieu.NgayHoanThanh = DateTime.Now;
            List<SanPham> sanPhams = new List<SanPham>();
            // cập nhật lại số lượng
            foreach (var item in phieu.ChiTietPhieuCheBiens)
            {
                var donViTinhPhieu = item.DonViTinh;
                var soLuongPhieu = (long)item.SoLuong;

                var giatriCBSP = item.CongThuc.SanPham.DonViTinhs
                    .Where(x => x.Ten == donViTinhPhieu).Select(x => x.GiaTriChuyenDoi).First();
                // 10kg -> dvt cb g .. 10 ... 10/1 = 10 1 bit
                var soLuongChuyenDoi = soLuongPhieu * giatriCBSP;

                var soLuongSanPham = item.CongThuc.SanPham.SoLuong;

                var soLuongMoi = soLuongSanPham + soLuongChuyenDoi;

                var sanPham = await _context.SanPhams.FindAsync(item.CongThuc.IdSanPham);
                sanPham.SoLuong = soLuongMoi;
                sanPhams.Add(sanPham);
            }

            _context.SanPhams.UpdateRange(sanPhams);
            _context.PhieuCheBiens.Update(phieu);
            await _context.SaveChangesAsync();

            //Khởi Tạo Thông báo
            var idngNhan = await _context.KeHoachCheBiens.FindAsync(phieu.IdKeHoach);
            var ngNhan = await _userManager.FindByIdAsync(idngNhan.IdNguoiNhan.ToString());
            var thongBao = new TaoThongBao()
            {
                IdChiMuc = phieu.IdChiMuc,
                DuongDan = "/NhiemVuPhieuCheBien/Index",
                Ten = "Cập nhật phiếu chế biến: " + phieu.MaSo,
                NguoiNhan = ngNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanPhieuCheBien,
                IdNguoiGui = (Guid)phieu.IdNguoiDuyet,
                MaKeHoach = idngNhan.MaSo
            };
            var idThongBao = await _thongBaoService.Create(thongBao);

            var ngTao = await _userManager.FindByIdAsync(phieu.IdNguoiTao.ToString());

            var result = new NhanKetQuaThongBaoCB()
            {
                Id = phieu.Id,
                MaPhieu = phieu.MaSo,
                MaKeHoach = idngNhan.MaSo,
                NgayTao = phieu.NgayTao.ToString("dd-MM-yyyy"),
                UserName = ngNhan.UserName,
                TrangThai = "Đã hoàn thành",
                NguoiTao = ngTao.MaSo,
                NguoiNhan = ngNhan.MaSo,
                IdThongBao = idThongBao.ResultObj
            };

            return new ApiSuccessResult<NhanKetQuaThongBaoCB>(result);
        }

        public async Task<ApiResult<NhanKetQuaThongBaoCB>> Create(TaoPhieuCheBien bundle)
        {
            List<ChiTietCheBien> chiTietCheBienUpdates = new List<ChiTietCheBien>();
            List<NguyenVatLieu> nguyenVatLieus = new List<NguyenVatLieu>();
            // cập nhật lại số lượng đã thêm trong chi tiết chế biến và nguyên vật liệu
            int i = 0;
            foreach (var item in bundle.IdChiTiet)
            {
                //--Chi tiết chế biến
                var chiTietCB = await _context.ChiTietCheBiens.FindAsync(item);

                // cập nhật lại số lượng đã thêm
                var soLuongDaThem = chiTietCB.SoLuongDaThem + bundle.SoLuong[i];
                // kiểm tra xem số lượng nhập vào có full chưa
                chiTietCB.SoLuongDaThem = soLuongDaThem;
                chiTietCheBienUpdates.Add(chiTietCB);
                if (chiTietCB.SoLuong <= soLuongDaThem)
                {
                    chiTietCB.TrangThaiChiTiet = TrangThaiChiTiet.DaHoanThanh;
                }
                //--Nguyên vật liệu
                var dsCT = await _context.ChiTietCongThucs.Include(x => x.NguyenVatLieu)
                    .ThenInclude(x => x.DonViTinhs).Where(x => x.IdCongThuc == bundle.IdCongThuc[i]).ToListAsync();
                foreach (var nvl in dsCT)
                {
                    var soLuongCT = nvl.SoLuong;
                    var soLuongNVL = nvl.NguyenVatLieu.SoLuong;
                    var giaTriDVTChung = nvl.NguyenVatLieu.DonViTinhs
                        .Where(x => x.Ten == nvl.DonViTinh)
                        .Select(x => x.GiaTriChuyenDoi).First();
                    var soLuongThat = (long)(soLuongNVL / giaTriDVTChung);
                    var soLuongDu = soLuongNVL - (soLuongThat * giaTriDVTChung);
                    // số lượng tiêu
                    var soLuongTieu = nvl.SoLuong * bundle.SoLuong[i];
                    // trừ số lượng đã tiêu trong NVL
                    var soLuongMoiNVL = soLuongThat - soLuongTieu;
                    if (soLuongMoiNVL < 0)
                    {
                        return new ApiErrorResult<NhanKetQuaThongBaoCB>("Thêm thất bại vì số lượng nguyên vật liệu đã hết");
                    }
                    // đổi số lượng NVL về cơ bản
                    var soLuongLuu = soLuongMoiNVL * giaTriDVTChung + soLuongDu;
                    var capNhatNVL = await _context.NguyenVatLieus.FindAsync(nvl.IdNguyenVatLieu);
                    capNhatNVL.SoLuong = soLuongLuu;
                    nguyenVatLieus.Add(capNhatNVL);
                }
                i++;
            }

            // khởi tạo chi tiết phiếu
            List<ChiTietPhieuCheBien> danhSachPhieuChiTiet = new List<ChiTietPhieuCheBien>();
            int sl = 0;
            foreach (var item in bundle.SoLuong)
            {
                if (item != 0)
                {
                    var ketQua = new ChiTietPhieuCheBien()
                    {
                        IdCongThuc = bundle.IdCongThuc[sl],
                        DonViTinh = bundle.DonViTinh[sl],
                        SoLuong = item
                    };
                    danhSachPhieuChiTiet.Add(ketQua);
                }
                sl++;
            }

            // tạo code
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var location = code.ViTri + stt;

            var strCode = code.Ten + location;

            var checkCode = await _context.PhieuCheBiens.AnyAsync(x => x.MaSo == strCode);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = location;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();
            var idNguoiTao = await _userManager.FindByNameAsync(bundle.NguoiTao);

            // khởi tạo phiếu chế biến
            var phieuCB = new PhieuCheBien()
            {
                IdChiMuc = bundle.IdChiMuc,
                IdKeHoach = bundle.IdKeHoach,
                IdNguoiTao = idNguoiTao.Id,
                MaSo = strCode,
                NgayTao = DateTime.Now,
                ChiTietPhieuCheBiens = danhSachPhieuChiTiet
            };

            _context.PhieuCheBiens.Add(phieuCB);
            await _context.SaveChangesAsync();

            // cập nhật lại kế hoạch
            // kiểm tra xem kế hoạch có chi tiết đã hoàn thành hết chưa.
            var keHoach = await _context.KeHoachCheBiens.Include(x => x.ChiTietCheBiens).Where(x => x.Id == bundle.IdKeHoach).FirstAsync();
            bool giaTri = false;
            foreach (var kiemTra in keHoach.ChiTietCheBiens)
            {
                if (kiemTra.TrangThaiChiTiet != TrangThaiChiTiet.ChuaHoanThanh)
                {
                    giaTri = true;
                }
                else
                {
                    giaTri = false;
                    break;
                }
            }
            // gia trị = true thì cập nhật lại trạng thái
            if (giaTri)
            {
                keHoach.TrangThai = TrangThaiKeHoach.HoanThanh;
                _context.KeHoachCheBiens.Update(keHoach);
                await _context.SaveChangesAsync();
            }
            var idNguoiNhan = await _userManager.FindByIdAsync(keHoach.IdNguoiNhan.ToString());

            var result = new NhanKetQuaThongBaoCB()
            {
                GiaTri = giaTri,
                IdKeHoach = bundle.IdKeHoach,
                MaKeHoach = keHoach.MaSo,
                MaPhieu = phieuCB.MaSo,
                NgayTao = phieuCB.NgayTao.ToString("dd-MM-yyyy"),
                ThongDiep = "Nhận phiếu chế biến " + phieuCB.MaSo,
                TrangThai = "Chưa hoàn thành",
                NguoiTao = bundle.NguoiTao,
                NguoiNhan = idNguoiNhan.MaSo,
                Id = phieuCB.Id,
                UserName = idNguoiNhan.UserName
            };

            var thongBao = new TaoThongBao()
            {
                IdChiMuc = bundle.IdChiMuc,
                DuongDan = "/NhiemVuPhieuCheBien/Index",
                Ten = "Nhận phiếu chế biến: " + phieuCB.MaSo,
                NguoiNhan = idNguoiNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanPhieuCheBien,
                IdNguoiGui = idNguoiTao.Id,
                MaKeHoach = keHoach.MaSo
            };
            var idThongBao = await _thongBaoService.Create(thongBao);

            result.IdThongBao = idThongBao.ResultObj;
            return new ApiSuccessResult<NhanKetQuaThongBaoCB>(result);
        }

        public async Task<List<DanhSachCheBien>> GetAllDanhSachChebien(long id)
        {
            var ds = _context.ChiTietCheBiens.Include(x => x.CongThuc).ThenInclude(x => x.SanPham)
                .Where(x => x.IdKeHoachCheBien == id
                && x.TrangThaiChiTiet == TrangThaiChiTiet.ChuaHoanThanh);
            var result = await ds.Select(x => new DanhSachCheBien()
            {
                IdChiTiet = x.Id,
                DonViTinh = x.DonViTinh,
                SoLuong = x.SoLuong,
                SoLuongDaThem = x.SoLuongDaThem,
                MaSoSanPham = x.CongThuc.SanPham.MaSo,
                TenSanPham = x.CongThuc.SanPham.Ten,
                MaSoCongThuc = x.CongThuc.MaSo
            }).ToListAsync();
            return new List<DanhSachCheBien>(result);
        }

        public async Task<List<KeHoachCheBienModel>> GetAllKeHoachCheBienChuaHoanThanh(TimKiemCheBien bundle)
        {
            var cheBien = _context.KeHoachCheBiens.Where(x => x.NhanKeHoach == true && x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh);

            if (!String.IsNullOrEmpty(bundle.TuKhoa))
            {
                cheBien = cheBien.Where(x => x.MaSo.Contains(bundle.TuKhoa) || x.Ten.Contains(bundle.TuKhoa));
            }
            if (!String.IsNullOrEmpty(bundle.Ngay))
            {
                var str = bundle.Ngay.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                cheBien = cheBien.Where(x => x.NgayBatDauDuKien.Date >= ngayBatDauDuKien.Date
                && x.NgayKetThucDuKien.Date <= ngayKetThucDuKien.Date);
            }
            cheBien = cheBien.Include(x => x.NguoiTao);
            cheBien = cheBien.Include(x => x.NguoiNhan);
            var result = await cheBien
                .Select(x => new KeHoachCheBienModel
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MaSo = x.MaSo,
                    NgayTao = x.NgayTao.ToString("dd/MM/yyyy"),
                    NguoiTao = x.NguoiTao.MaSo,
                    NguoiNhan = x.NguoiNhan.MaSo,
                    TrangThai = x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh ? "Chưa hoàn thành" :
                                x.TrangThai == TrangThaiKeHoach.HoanThanh ? "Hoàn thành" : "Bị hủy"
                }).ToListAsync();

            return new List<KeHoachCheBienModel>(result);
        }

        public async Task<ApiResult<PagedResult<PhieuCheBienModel>>> GetAllPhieuCheBien(TimKiemPhieuCheBien bundle)
        {
            var phieu = _context.PhieuCheBiens.Include(x => x.ChiTietPhieuCheBiens)
                       .Where(x => x.TrangThaiPhieu != TrangThaiPhieu.ChuaHoanThanh);
            phieu = phieu.Include(x => x.KeHoachCheBien).ThenInclude(x => x.NguoiNhan);

            if (!String.IsNullOrEmpty(bundle.TuKhoa))
            {
                phieu = phieu.Where(x => x.MaSo.Contains(bundle.TuKhoa) || x.KeHoachCheBien.MaSo.Contains(bundle.TuKhoa));
            }
            if (!String.IsNullOrEmpty(bundle.Ngay))
            {
                var str = bundle.Ngay.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                phieu = phieu.Where(x => x.NgayTao.Date >= ngayBatDauDuKien.Date && x.NgayTao.Date <= ngayKetThucDuKien.Date);
            }

            if (bundle.TrangThai > 0)
            {
                switch (bundle.TrangThai)
                {
                    case 1:
                        phieu = phieu.Where(x => x.TrangThaiPhieu == TrangThaiPhieu.HoanThanh);
                        break;

                    case 2:
                        phieu = phieu.Where(x => x.TrangThaiPhieu == TrangThaiPhieu.BiHuy);
                        break;
                }
            }
            phieu = phieu.Include(x => x.NguoiTao);

            var totalRow = await phieu.CountAsync();

            var result = await phieu
            .Skip((bundle.PageIndex - 1) * bundle.PageSize)
            .Take(bundle.PageSize)
             .Select(x => new PhieuCheBienModel
             {
                 Id = x.Id,
                 MaSo = x.MaSo,
                 NguoiNhan = x.KeHoachCheBien.NguoiNhan.MaSo,
                 MaKeHoach = x.KeHoachCheBien.MaSo,
                 NgayTao = x.NgayTao.ToString("dd/MM/yyyy"),
                 TrangThai = x.TrangThaiPhieu != TrangThaiPhieu.BiHuy ? "Đã hoàn thành" : "Bị hủy",
                 NguoiTao = x.NguoiTao.MaSo
             }).ToListAsync();

            var pagedResult = new PagedResult<PhieuCheBienModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<PhieuCheBienModel>>(pagedResult);
        }

        public async Task<List<CongThucModelKeHoach>> GetCongThucKeHoach(long id)
        {
            var process = _context.ChiTietCheBiens.Include(x => x.CongThuc)
                .ThenInclude(x => x.ChiTietCongThucs).ThenInclude(x => x.NguyenVatLieu)
                .Where(x => x.IdKeHoachCheBien == id && x.TrangThaiChiTiet == TrangThaiChiTiet.ChuaHoanThanh);

            process = process.Include(x => x.CongThuc.SanPham);

            var result = await process.Select(x => new CongThucModelKeHoach()
            {
                IdChiTiet = x.Id,
                MaSanPham = x.CongThuc.SanPham.MaSo,
                TenSanPham = x.CongThuc.SanPham.Ten,
                SoLuong = x.SoLuong,
                SoLuongDaThem = x.SoLuongDaThem,
                DonViTinh = x.DonViTinh,
                IdCongThuc = x.IdCongThuc,
                MaCongThuc = x.CongThuc.MaSo,
                DanhSachNguyenVatLieus = x.CongThuc.ChiTietCongThucs.Select(r => new DanhSachNguyenVatLieu()
                {
                    IdNguyenVatLieu = r.IdNguyenVatLieu,
                    MaNguyenVatLieu = r.NguyenVatLieu.MaSo,
                    TenNguyenVatLieu = r.NguyenVatLieu.Ten,
                    SoLuongHienCoNVL = r.NguyenVatLieu.SoLuong,
                    SoLuongCanCT = r.SoLuong,
                    DonViTinh = r.DonViTinh,
                    GiaTriChuyenDoi = r.NguyenVatLieu.DonViTinhs
                    .Where(x => x.IdNguyenVatLieu == r.IdNguyenVatLieu && x.Ten == r.DonViTinh)
                    .Select(x => x.GiaTriChuyenDoi)
                    .First()
                }).ToList()
            }).ToListAsync();

            return new List<CongThucModelKeHoach>(result);
        }

        public async Task<ApiResult<ChiTietPhieuCheBienModel>> GetIdInforCheBien(long id)
        {
            var phieu = _context.PhieuCheBiens.Where(x => x.Id == id);

            phieu = phieu.Include(x => x.KeHoachCheBien).ThenInclude(x => x.NguoiNhan);
            phieu = phieu.Include(x => x.NguoiTao);
            phieu = phieu.Include(x => x.NguoiDuyet);
            phieu = phieu.Include(x => x.ChiTietPhieuCheBiens).ThenInclude(x => x.CongThuc).ThenInclude(x => x.SanPham);
            var result = await phieu.Select(x => new ChiTietPhieuCheBienModel()
            {
                Id = x.Id,
                MaSo = x.MaSo,
                MaKeHoach = x.KeHoachCheBien.MaSo,
                NgayTao = x.NgayTao.ToString("dd-MM-yyyy"),
                NgayHoanThanh = x.TrangThaiPhieu != TrangThaiPhieu.ChuaHoanThanh ? x.NgayHoanThanh.ToString("dd-MM-yyyy") : " ",
                NguoiTao = x.NguoiTao.MaSo,
                NguoiNhan = x.KeHoachCheBien.NguoiNhan.MaSo,
                NguoiDuyet = x.IdNguoiDuyet != null ? x.NguoiDuyet.MaSo : " ",
                DanhSachPhieuChiTietCheBiens = x.ChiTietPhieuCheBiens
                .Select(c => new DanhSachPhieuChiTietCheBien()
                {
                    Id = c.Id,
                    DonViTinh = c.DonViTinh,
                    SoLuong = c.SoLuong,
                    MaCongThuc = c.CongThuc.MaSo,
                    MaSanPham = c.CongThuc.SanPham.MaSo,
                    TenSanPham = c.CongThuc.SanPham.Ten
                }).ToList()
            }).FirstAsync();

            return new ApiSuccessResult<ChiTietPhieuCheBienModel>(result);
        }

        public async Task<List<PhieuCheBienModel>> GetPhieuCheBienDangCho(TimKiemCheBien bundle)
        {
            var phieu = _context.PhieuCheBiens.Include(x => x.ChiTietPhieuCheBiens)
                        .Where(x => x.TrangThaiPhieu == TrangThaiPhieu.ChuaHoanThanh);

            phieu = phieu.Include(x => x.KeHoachCheBien).ThenInclude(x => x.NguoiNhan);

            if (!String.IsNullOrEmpty(bundle.TuKhoa))
            {
                phieu = phieu.Where(x => x.MaSo.Contains(bundle.TuKhoa) && x.KeHoachCheBien.MaSo.Contains(bundle.TuKhoa));
            }
            if (!String.IsNullOrEmpty(bundle.Ngay))
            {
                var str = bundle.Ngay.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                phieu = phieu.Where(x => x.NgayTao.Date >= ngayBatDauDuKien.Date && x.NgayTao.Date <= ngayKetThucDuKien.Date);
            }
            phieu = phieu.Include(x => x.NguoiTao);

            var result = await phieu
             .Select(x => new PhieuCheBienModel
             {
                 Id = x.Id,
                 MaSo = x.MaSo,
                 NguoiNhan = x.KeHoachCheBien.NguoiNhan.MaSo,
                 MaKeHoach = x.KeHoachCheBien.MaSo,
                 NgayTao = x.NgayTao.ToString("dd/MM/yyyy"),
                 NguoiTao = x.NguoiTao.MaSo
             }).ToListAsync();
            return new List<PhieuCheBienModel>(result);
        }

        public async Task<ApiResult<bool>> HuyChiTietCheBien(long id)
        {
            var cheBien = await _context.ChiTietCheBiens.FindAsync(id);
            if (cheBien == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            if (cheBien.SoLuongDaThem == 0)
            {
                cheBien.TrangThaiChiTiet = TrangThaiChiTiet.BiHuy;
            }
            else
            {
                cheBien.TrangThaiChiTiet = TrangThaiChiTiet.BiHuyMotPhan;
            }
            _context.ChiTietCheBiens.Update(cheBien);
            await _context.SaveChangesAsync();

            // xem chi tiết có cái nào chưa chế biến ko
            var keHoach = await _context.KeHoachCheBiens.Include(x => x.ChiTietCheBiens)
                .Where(x => x.Id == cheBien.IdKeHoachCheBien).FirstAsync();
            bool giaTri = false;
            foreach (var kiemTra in keHoach.ChiTietCheBiens)
            {
                if (kiemTra.TrangThaiChiTiet != TrangThaiChiTiet.ChuaHoanThanh)
                {
                    giaTri = true;
                }
                else
                {
                    giaTri = false;
                    break;
                }
            }
            // gia trị = true thì cập nhật lại trạng thái
            if (giaTri)
            {
                keHoach.TrangThai = TrangThaiKeHoach.HoanThanh;
                _context.KeHoachCheBiens.Update(keHoach);
                await _context.SaveChangesAsync();
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<NhanKetQuaThongBaoCB>> HuyPhieuCheBien(long id, string nguoiDuyet)
        {
            var phieu = await _context.PhieuCheBiens.FindAsync(id);
            if (phieu == null)
            {
                return new ApiErrorResult<NhanKetQuaThongBaoCB>("Không tồn tại id");
            }
            phieu.TrangThaiPhieu = TrangThaiPhieu.BiHuy;
            phieu.NgayHoanThanh = DateTime.Now;

            var ngDuyet = await _userManager.FindByNameAsync(nguoiDuyet);
            phieu.IdNguoiDuyet = ngDuyet.Id;

            _context.PhieuCheBiens.Update(phieu);
            await _context.SaveChangesAsync();
            var idngNhan = await _context.KeHoachCheBiens.FindAsync(phieu.IdKeHoach);
            var ngNhan = await _userManager.FindByIdAsync(idngNhan.IdNguoiNhan.ToString());
            var thongBao = new TaoThongBao()
            {
                IdChiMuc = phieu.IdChiMuc,
                DuongDan = "/NhiemVuPhieuCheBien/Index",
                Ten = "Hủy phiếu chế biến: " + phieu.MaSo,
                NguoiNhan = ngNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanPhieuCheBien,
                IdNguoiGui = (Guid)phieu.IdNguoiDuyet,
                MaKeHoach = idngNhan.MaSo
            };
            var idThongBao = await _thongBaoService.Create(thongBao);

            var ngTao = await _userManager.FindByIdAsync(phieu.IdNguoiTao.ToString());

            var result = new NhanKetQuaThongBaoCB()
            {
                Id = phieu.Id,
                MaPhieu = phieu.MaSo,
                MaKeHoach = idngNhan.MaSo,
                NgayTao = phieu.NgayTao.ToString("dd-MM-yyyy"),
                UserName = ngNhan.UserName,
                TrangThai = "Đã hủy",
                NguoiTao = ngTao.MaSo,
                NguoiNhan = ngNhan.MaSo,
                IdThongBao = idThongBao.ResultObj
            };
            return new ApiSuccessResult<NhanKetQuaThongBaoCB>(result);
        }
    }
}