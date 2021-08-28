using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.ViewModels.CheBien;
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

namespace QuanLyKho.Service.KeHoachDatHangService
{
    public class KeHoachDatHangService : IKeHoachDatHangService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IThongBaoService _thongBaoService;

        public KeHoachDatHangService(QuanLyKhoDbContext context, UserManager<AppUser> userManager, IThongBaoService thongBaoService)
        {
            _context = context;
            _userManager = userManager;
            _thongBaoService = thongBaoService;
        }

        public async Task<ApiResult<KeHoachCheBienModel>> GetInfoKeHoachCheBienDuKien(long id)
        {
            var cheBien = _context.KeHoachCheBiens.Include(x => x.ChiTietCheBiens)
                .ThenInclude(x => x.CongThuc).ThenInclude(x => x.SanPham)
                .Where(x => x.Id == id);
            var ketQua = await cheBien
                .Select(x => new KeHoachCheBienModel()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    NgayDuKienBatDau = x.NgayBatDauDuKien.ToString("dd-MM-yyyy"),
                    NhanKeHoach = x.NhanKeHoach == true ? "Đã nhận" : "Chưa nhận",
                    NguoiTao = x.NguoiTao.MaSo,
                    DanhSachCheBiens = x.ChiTietCheBiens
                    .Select(x => new DanhSachCheBien()
                    {
                        IdChiTiet = x.Id,
                        DonViTinh = x.DonViTinh,
                        GhiChu = x.GhiChu,
                        SoLuong = x.SoLuong,
                        MaSoCongThuc = x.CongThuc.MaSo,
                        TenSanPham = x.CongThuc.SanPham.Ten,
                        TrangThai = x.TrangThaiChiTiet == TrangThaiChiTiet.ChuaHoanThanh ? "Chưa chế biến" : "Đã chế biến xong"
                    }).ToList()
                }).FirstOrDefaultAsync();
            return new ApiSuccessResult<KeHoachCheBienModel>(ketQua);
        }

        public async Task<List<GetTenLoaiNVL>> GetLoaiNVLKH(int nhomNVL)
        {
            NhomLoaiNVL nhom = NhomLoaiNVL.NguyenLieu;
            switch (nhomNVL)
            {
                case 0:
                    nhom = NhomLoaiNVL.NguyenLieu;
                    break;

                case 1:
                    nhom = NhomLoaiNVL.VatLieu;
                    break;

                case 2:
                    nhom = NhomLoaiNVL.NhienLieu;
                    break;
            }
            var loaiNVL = _context.LoaiNguyenVatLieus.Where(x => x.NhomLoaiNVL == nhom);
            var ketQua = await loaiNVL
                .Select(x => new GetTenLoaiNVL()
                {
                    Id = x.Id,
                    Ten = x.Ten
                }).ToListAsync();
            return new List<GetTenLoaiNVL>(ketQua);
        }

        public async Task<List<NguyenVatLieuKH>> GetNguyenVatLieuKH(int idLoaiNVL, string tuKhoa)
        {
            var nguyenVatLieu = _context.NguyenVatLieus.Include(x => x.DonViTinhs).Where(x => x.IdLoaiNVL == idLoaiNVL);

            if (!String.IsNullOrEmpty(tuKhoa))
            {
                nguyenVatLieu = nguyenVatLieu.Where(x => x.MaSo.Contains(tuKhoa) || x.Ten.Contains(tuKhoa));
            }

            var ketQua = await nguyenVatLieu
                            .Select(x => new NguyenVatLieuKH()
                            {
                                Id = x.Id,
                                MaSo = x.MaSo,
                                Ten = x.Ten,
                                SoLuong = x.SoLuong,
                                TrangThaiTonKho = x.NhacNho == true ? (x.SoLuong >= x.MucTonCaoNhat ? true : false) : false,
                            }).ToListAsync();
            return new List<NguyenVatLieuKH>(ketQua);
        }

        public async Task<List<DonViTinhModel>> GetDonViTinhKHDH(int idNVL)
        {
            var donViTinh = await _context.DonViTinhs.Where(x => x.IdNguyenVatLieu == idNVL).ToListAsync();
            var result = donViTinh
                .OrderByDescending(x => x.CoBan)
                .Select(x => new DonViTinhModel()
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    GiaTriChuyenDoi = x.GiaTriChuyenDoi
                });
            return new List<DonViTinhModel>(result);
        }

        public async Task<List<DonViTinhModel>> GetShowDonViTinhKHDH(int idNVL)
        {
            var donViTinh = await _context.DonViTinhs.Where(x => x.IdNguyenVatLieu == idNVL).ToListAsync();
            var result = donViTinh
                .OrderByDescending(x => x.CoBan)
                .Select(x => new DonViTinhModel()
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    CoBan = x.CoBan,
                    GiaTriChuyenDoi = x.GiaTriChuyenDoi
                });
            return new List<DonViTinhModel>(result);
        }

        public async Task<List<KeHoachCheBienModel>> KeHoachCheBienDuKien()
        {
            var cheBien = _context.KeHoachCheBiens.Include(x => x.NguoiTao)
                .Where(x => x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh);
            var ketQua = await cheBien
                .Select(x => new KeHoachCheBienModel()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    Ten = x.Ten,
                    NgayTao = x.NgayTao.ToString("dd-MM-yyyy"),
                    NguoiTao = x.NguoiTao.MaSo
                }).ToListAsync();
            return new List<KeHoachCheBienModel>(ketQua);
        }

        public async Task<List<NhanVienKHDatHang>> NhanVienKHDatHang(string tuKhoa)
        {
            var nhanVien = _userManager.Users.Where(x => x.TinhTrangLamViec == TinhTrangLamViec.DangLamViec
                                                && x.LoaiTaiKhoan == LoaiTaiKhoan.NhanVien);
            if (!String.IsNullOrEmpty(tuKhoa))
            {
                nhanVien = nhanVien.Where(x => x.Ten.Contains(tuKhoa) || x.MaSo.Contains(tuKhoa) || x.CanCuocCongDan.Contains(tuKhoa));
            }
            var result = await nhanVien.Select(x => new NhanVienKHDatHang()
            {
                Id = x.Id,
                CanCuocCongDan = x.CanCuocCongDan,
                MaSo = x.MaSo,
                Ho = x.Ho,
                Ten = x.Ten
            }).ToListAsync();

            return new List<NhanVienKHDatHang>(result);
        }

        public async Task<ApiResult<NhanKetQuaDatHang>> Create(TaoKeHoachDatHang bundle)
        {
            var str = bundle.NgayDuKien.Split(" - ");
            DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
            DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
            // lưu chi tiết chế biến
            List<ChiTietDatHang> chiTietDatHangs = new List<ChiTietDatHang>();
            if (bundle.IdNguyenVatLieu == null || bundle.DonViTinh == null || bundle.SoLuong == null)
            {
                return new ApiErrorResult<NhanKetQuaDatHang>("Dữ liệu chi tiết đặt hàng không được bỏ trống");
            }
            int i = 0;
            foreach (var item in bundle.IdNguyenVatLieu)
            {
                var chiTiet = new ChiTietDatHang()
                {
                    IdNguyenVatLieu = item,
                    DonViTinh = bundle.DonViTinh[i],
                    SoLuong = bundle.SoLuong[i],
                    TrangThaiChiTiet = TrangThaiChiTiet.ChuaHoanThanh
                };

                chiTietDatHangs.Add(chiTiet);

                i++;
            }
            // tạo mã số
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var viTri = code.ViTri + stt;

            var maSo = code.Ten + viTri;

            var checkCode = await _context.KeHoachDatHangs.AnyAsync(x => x.MaSo == maSo);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = viTri;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            // Tìm người tạo
            var user = await _userManager.FindByNameAsync(bundle.NguoiTao);
            // lưu kế hoạch chế biến
            var keHoachDatHang = new KeHoachDatHang()
            {
                MaSo = maSo,
                GhiChu = bundle.GhiChu,
                IdNguoiNhan = bundle.IdNguoiNhan,
                IdNguoiTao = user.Id,
                IdChiMuc = bundle.IdChiMuc,
                NgayTao = DateTime.Now,
                NgayBatDauDuKien = ngayBatDauDuKien,
                NgayKetThucDuKien = ngayKetThucDuKien,
                Ten = bundle.Ten,
                TrangThai = TrangThaiKeHoach.ChuaHoanThanh,
                NhanKeHoach = false,
                ChiTietDatHangs = chiTietDatHangs
            };

            var datHang = await _context.KeHoachCheBiens.FindAsync(bundle.IdKeHoachCheBien);
            _context.KeHoachDatHangs.Add(keHoachDatHang);
            datHang.LenDatHang = true;
            _context.KeHoachCheBiens.Update(datHang);
            await _context.SaveChangesAsync();

            var ngNhan = await _userManager.Users.Where(x => x.Id == bundle.IdNguoiNhan).FirstAsync();
            var result = new NhanKetQuaDatHang()
            {
                Id = keHoachDatHang.Id,
                MaSo = keHoachDatHang.MaSo,
                Ten = keHoachDatHang.Ten,
                NguoiNhan = ngNhan.UserName,
                NgayTao = keHoachDatHang.NgayTao.ToString("dd/MM/yyyy"),
                NguoiTao = bundle.NguoiTao
            };

            //Khởi Tạo Thông báo

            var thongBao = new TaoThongBao()
            {
                IdChiMuc = bundle.IdChiMuc,
                DuongDan = "/NhiemVuKeHoachDatHang/Index",
                Ten = "Nhận kế hoạch đặt hàng: " + result.MaSo,
                NguoiNhan = ngNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanKeHoachDatHang,
                IdNguoiGui = user.Id,
                MaKeHoach = keHoachDatHang.MaSo
            };
            var idThongBao = await _thongBaoService.Create(thongBao);
            result.IdThongBao = idThongBao.ResultObj;

            return new ApiSuccessResult<NhanKetQuaDatHang>(result);
        }

        public async Task<List<NhanKetQuaDatHang>> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var keHoachDatHang = _context.KeHoachDatHangs
                               .Where(x => (x.NgayBatDauDuKien.Month == thang && x.NgayBatDauDuKien.Year == nam)
                               || (x.NgayKetThucDuKien.Month == thang && x.NgayKetThucDuKien.Year == nam));
            var result = await keHoachDatHang
                            .Select(x => new NhanKetQuaDatHang()
                            {
                                Id = x.Id,
                                MaSo = x.MaSo,
                                Ten = x.Ten,
                                NhanKeHoach = x.NhanKeHoach,
                                NgayBatDauDuKien = x.NgayBatDauDuKien.ToString("yyyy-MM-dd"),
                                NgayKetThucDuKien = x.NgayKetThucDuKien.AddDays(1).ToString("yyyy-MM-dd"),
                                TrangThai = x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh ? "ChuaHoanThanh" :
                                x.TrangThai == TrangThaiKeHoach.HoanThanh ? "HoanThanh" : "BiHuy"
                            }).ToListAsync();

            return new List<NhanKetQuaDatHang>(result);
        }

        public async Task<ApiResult<KeHoachDatHangModel>> GetInfoKeHoachById(long id)
        {
            var keHoach = await _context.KeHoachDatHangs.Include(x => x.ChiTietDatHangs)
                 .ThenInclude(c => c.NguyenVatLieu)
                 .Where(x => x.Id == id).FirstOrDefaultAsync();

            var result = new KeHoachDatHangModel()
            {
                Id = keHoach.Id,
                Ten = keHoach.Ten,
                MaSo = keHoach.MaSo,
                GhiChu = keHoach.GhiChu,
                IdNguoiNhan = keHoach.IdNguoiNhan,
                NgayTao = keHoach.NgayTao.ToString("dd/MM/yyyy"),
                NgayDuKienBatDau = keHoach.NgayBatDauDuKien.ToString("dd/MM/yyyy"),
                NgayDuKienKetThuc = keHoach.NgayKetThucDuKien.ToString("dd/MM/yyyy"),
                NgayDuKienBatDauEdit = keHoach.NgayBatDauDuKien.ToString("MM/dd/yyyy"),
                NgayDuKienKetThucEdit = keHoach.NgayKetThucDuKien.ToString("MM/dd/yyyy"),
                NhanKeHoach = keHoach.NhanKeHoach == true ? "Đã nhận kế hoạch" : "Chưa nhận kế hoạch",
                TrangThai = keHoach.TrangThai == TrangThaiKeHoach.ChuaHoanThanh ? "Chưa hoàn thành" :
                                keHoach.TrangThai == TrangThaiKeHoach.HoanThanh ? "Hoàn thành" : "Bị hủy"
            };
            // người nhận
            var ngNhan = _userManager.Users.Where(x => x.Id == keHoach.IdNguoiNhan).FirstOrDefault();
            result.NguoiNhan = ngNhan.MaSo;

            // người tạo
            var ngTao = _userManager.Users.Where(x => x.Id == keHoach.IdNguoiTao).FirstOrDefault();
            result.NguoiTao = ngTao.MaSo;

            // chi tiết danh sách sản phẩm
            List<DanhSachDatHang> dsDatHangs = new List<DanhSachDatHang>();
            foreach (var item in keHoach.ChiTietDatHangs)
            {
                var maNCC = "";
                if (item.IdNhaCungCap != null)
                {
                    maNCC = await _context.NhaCungCaps.Where(x => x.Id == item.IdNhaCungCap).Select(x => x.MaSo).FirstOrDefaultAsync();
                }
                var danhSach = new DanhSachDatHang()
                {
                    IdNguyenVatLieu = item.IdNguyenVatLieu,
                    DonViTinh = item.DonViTinh,
                    IdChiTiet = item.Id,
                    GhiChu = item.GhiChu,
                    SoLuong = item.SoLuong,
                    MaSoNguyenVatLieu = item.NguyenVatLieu.MaSo,
                    TenNguyenVatLieu = item.NguyenVatLieu.Ten,
                    TrangThai = item.TrangThaiChiTiet == TrangThaiChiTiet.BiHuy ? "Bị hủy" :
                    item.TrangThaiChiTiet == TrangThaiChiTiet.BiHuyMotPhan ? "Bị hủy một phần" :
                    item.TrangThaiChiTiet == TrangThaiChiTiet.DaHoanThanh ? "Đã hoàn thành" : "Chưa hoàn thành",
                    MaSoNCC = maNCC
                };
                if (item.IdNhaCungCap != null)
                {
                    danhSach.IdNhaCungCap = (int)item.IdNhaCungCap;
                }
                else
                {
                    danhSach.IdNhaCungCap = 0;
                }
                dsDatHangs.Add(danhSach);
            }
            result.DanhSachDatHangs = dsDatHangs;
            return new ApiSuccessResult<KeHoachDatHangModel>(result);
        }

        public async Task<ApiResult<ThongBaoModel>> Delete(long id)
        {
            var keHoachDatHang = await _context.KeHoachDatHangs.Include(x => x.ChiTietDatHangs).FirstAsync(x => x.Id == id);
            if (keHoachDatHang.NhanKeHoach)
            {
                return new ApiErrorResult<ThongBaoModel>("Xóa thật bại vì kế hoạch đã thực thi");
            }
            var maSo = keHoachDatHang.MaSo;
            var chiMuc = keHoachDatHang.IdChiMuc;
            var nguoiNhan = await _userManager.FindByIdAsync(keHoachDatHang.IdNguoiNhan.ToString());
            var maKeHoach = keHoachDatHang.MaSo;
            var nguoiGui = keHoachDatHang.IdNguoiTao;
            _context.ChiTietDatHangs.RemoveRange(keHoachDatHang.ChiTietDatHangs);
            _context.KeHoachDatHangs.Remove(keHoachDatHang);
            await _context.SaveChangesAsync();

            var thongBao = new TaoThongBao()
            {
                IdChiMuc = chiMuc,
                DuongDan = "/NhiemVuKeHoachDatHang/Index",
                Ten = "Xóa kế hoạch đặt hàng: " + maSo,
                NguoiNhan = nguoiNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanKeHoachDatHang,
                MaKeHoach = maKeHoach,
                IdNguoiGui = nguoiGui
            };
            var idThongBao = await _thongBaoService.Create(thongBao);
            var ketQua = new ThongBaoModel()
            {
                Id = idThongBao.ResultObj,
                DuongDan = thongBao.DuongDan,
                Ten = thongBao.Ten,
                NguoiNhan = nguoiNhan.UserName
            };
            return new ApiSuccessResult<ThongBaoModel>(ketQua);
        }

        public async Task<ApiResult<bool>> DeleteNguyenVatLieu(int id)
        {
            var chiTiet = await _context.ChiTietDatHangs.FindAsync(id);
            if (chiTiet == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            _context.ChiTietDatHangs.Remove(chiTiet);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<NhanKetQuaDatHang>> Update(CapNhatKeHoachDatHang bundle)
        {
            var keHoachDatHang = await _context.KeHoachDatHangs.Include(x => x.ChiTietDatHangs).FirstAsync(x => x.Id == bundle.Id);
            if (keHoachDatHang.NhanKeHoach)
            {
                return new ApiErrorResult<NhanKetQuaDatHang>("Cập nhật thật bại vì kế hoạch đã thực thi");
            }
            bool giaoKeHoach = false;
            var idNguoiCu = keHoachDatHang.IdNguoiNhan;
            if (keHoachDatHang.IdNguoiNhan != bundle.IdNguoiNhan)
            {
                giaoKeHoach = true;
            }
            var str = bundle.NgayDuKien.Split(" - ");
            DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
            DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);

            keHoachDatHang.Ten = bundle.Ten;
            keHoachDatHang.NgayBatDauDuKien = ngayBatDauDuKien;
            keHoachDatHang.NgayKetThucDuKien = ngayKetThucDuKien;
            keHoachDatHang.IdNguoiNhan = bundle.IdNguoiNhan;
            keHoachDatHang.GhiChu = bundle.GhiChu;

            //cập nhật lại chi tiết chế biến
            List<ChiTietDatHang> chiTietDatHangCapNhats = new List<ChiTietDatHang>();
            if (bundle.IdChiTiet != null && bundle.IdNguyenVatLieu != null)
            {
                int i = 0;
                foreach (var item in bundle.IdChiTiet)
                {
                    var chiTiet = await _context.ChiTietDatHangs.FindAsync(item);
                    chiTiet.IdNguyenVatLieu = bundle.IdNguyenVatLieu[i];
                    chiTiet.SoLuong = bundle.SoLuong[i];
                    chiTiet.GhiChu = bundle.GhiChuDatHang[i];
                    chiTiet.DonViTinh = bundle.DonViTinh[i];
                    chiTietDatHangCapNhats.Add(chiTiet);

                    i++;
                }
                _context.ChiTietDatHangs.UpdateRange(chiTietDatHangCapNhats);
            }

            // thêm mới chi tiết chế biến
            List<ChiTietDatHang> chiTietDatHangThemMois = new List<ChiTietDatHang>();
            if (bundle.IdNguyenVatLieuThem != null && bundle.SoLuongThem != null && bundle.DonViTinhThem != null)
            {
                int i = 0;
                foreach (var item in bundle.IdNguyenVatLieuThem)
                {
                    var chiTietThem = new ChiTietDatHang()
                    {
                        IdNguyenVatLieu = item,
                        SoLuong = bundle.SoLuongThem[i],
                        GhiChu = bundle.GhiChuDatHangThem[i],
                        DonViTinh = bundle.DonViTinhThem[i],
                        IdKeHoachDatHang = bundle.Id
                    };

                    chiTietDatHangThemMois.Add(chiTietThem);
                    i++;
                }
                _context.ChiTietDatHangs.AddRange(chiTietDatHangThemMois);
            }

            _context.KeHoachDatHangs.Update(keHoachDatHang);
            await _context.SaveChangesAsync();

            var result = new NhanKetQuaDatHang()
            {
                Id = keHoachDatHang.Id,
                MaSo = keHoachDatHang.MaSo,
                Ten = keHoachDatHang.Ten,
                NgayBatDauDuKien = keHoachDatHang.NgayBatDauDuKien.ToString("yyyy-MM-dd"),
                NgayKetThucDuKien = keHoachDatHang.NgayKetThucDuKien.AddDays(1).ToString("yyyy-MM-dd")
            };
            result.ThongBaoModelOld = new List<ThongBaoModel>();
            result.ThongBaoModelNew = new List<ThongBaoModel>();
            if (giaoKeHoach)
            {
                var nguoiNhanNew = await _userManager.FindByIdAsync(keHoachDatHang.IdNguoiNhan.ToString());
                var nguoiNhanOld = await _userManager.FindByIdAsync(idNguoiCu.ToString());

                // thông báo người củ kế hoạch đã bị giao
                var thongBaoOld = new TaoThongBao()
                {
                    IdChiMuc = keHoachDatHang.IdChiMuc,
                    DuongDan = "/NhiemVuKeHoachDatHang/Index",
                    Ten = "Đã chuyển kế hoạch chế biến: " + result.MaSo,
                    NguoiNhan = nguoiNhanOld.UserName,
                    LoaiThongBao = LoaiThongBao.NhanKeHoachDatHang,
                    IdNguoiGui = keHoachDatHang.IdNguoiTao,
                    MaKeHoach = result.MaSo
                };
                var idThongBaoOld = await _thongBaoService.Create(thongBaoOld);
                var ketQuaOld = new ThongBaoModel()
                {
                    Id = idThongBaoOld.ResultObj,
                    DuongDan = thongBaoOld.Ten,
                    NguoiNhan = nguoiNhanOld.UserName,
                    Ten = thongBaoOld.Ten,
                    SoNgayNhan = 0
                };
                result.ThongBaoModelOld.Add(ketQuaOld);
                // thông báo người mới nhận kết hoạch
                var thongBaoNew = new TaoThongBao()
                {
                    IdChiMuc = keHoachDatHang.IdChiMuc,
                    DuongDan = "/NhiemVuKeHoachDatHang/Index",
                    Ten = "Nhận kế hoạch chế biến: " + result.MaSo,
                    NguoiNhan = nguoiNhanNew.UserName,
                    LoaiThongBao = LoaiThongBao.NhanKeHoachDatHang,
                    IdNguoiGui = keHoachDatHang.IdNguoiTao,
                    MaKeHoach = result.MaSo
                };
                var idThongBaoNew = await _thongBaoService.Create(thongBaoNew);

                var ketQuaNew = new ThongBaoModel()
                {
                    Id = idThongBaoNew.ResultObj,
                    DuongDan = thongBaoNew.Ten,
                    NguoiNhan = nguoiNhanNew.UserName,
                    Ten = thongBaoNew.Ten,
                    SoNgayNhan = 0
                };
                result.ThongBaoModelNew.Add(ketQuaNew);
            }
            else
            {
                var nguoiNhan = await _userManager.FindByIdAsync(keHoachDatHang.IdNguoiNhan.ToString());

                // thông báo cập nhật kế hoạch
                var thongBao = new TaoThongBao()
                {
                    IdChiMuc = keHoachDatHang.IdChiMuc,
                    DuongDan = "/NhiemVuKeHoachDatHang/Index",
                    Ten = "Cập nhật kế hoạch chế biến: " + result.MaSo,
                    NguoiNhan = nguoiNhan.UserName,
                    LoaiThongBao = LoaiThongBao.NhanKeHoachDatHang,
                    IdNguoiGui = keHoachDatHang.IdNguoiTao,
                    MaKeHoach = result.MaSo
                };
                var idThongBao = await _thongBaoService.Create(thongBao);
                var ketQua = new ThongBaoModel()
                {
                    Id = idThongBao.ResultObj,
                    DuongDan = thongBao.Ten,
                    NguoiNhan = nguoiNhan.UserName,
                    Ten = thongBao.Ten,
                    SoNgayNhan = 0
                };
                result.ThongBaoModelOld.Add(ketQua);
            }
            result.GiaoChoNguoiKhac = giaoKeHoach;
            return new ApiSuccessResult<NhanKetQuaDatHang>(result);
        }

        public async Task<ApiResult<PagedResult<KeHoachDatHangModel>>> GetAllKeHoach(GetAllKeHoachDatHang bundle)
        {
            var index = _context.KeHoachDatHangs;
            IQueryable<KeHoachDatHang> query = index.OrderByDescending(x => x.NgayTao);

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(x => x.Ten.Contains(bundle.TuKhoa) || x.MaSo.Contains(bundle.TuKhoa));
            }

            if (!string.IsNullOrEmpty(bundle.NgayDuKien))
            {
                var str = bundle.NgayDuKien.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                query = query.Where(x => x.NgayBatDauDuKien.Date >= ngayBatDauDuKien.Date
                && x.NgayKetThucDuKien.Date <= ngayKetThucDuKien.Date);
            }

            if (bundle.TrangThai > 0)
            {
                var trangThai = TrangThaiKeHoach.HoanThanh;
                switch (bundle.TrangThai)
                {
                    case 1:
                        trangThai = TrangThaiKeHoach.ChuaHoanThanh;
                        query = query.Where(x => x.TrangThai == trangThai && x.NhanKeHoach == false);
                        break;

                    case 2:
                        trangThai = TrangThaiKeHoach.ChuaHoanThanh;
                        query = query.Where(x => x.TrangThai == trangThai && x.NhanKeHoach == true);
                        break;

                    case 3:
                        trangThai = TrangThaiKeHoach.HoanThanh;
                        query = query.Where(x => x.TrangThai == trangThai);
                        break;

                    case 4:
                        trangThai = TrangThaiKeHoach.BiHuy;
                        query = query.Where(x => x.TrangThai == trangThai);
                        break;
                }
            }

            query = query.Include(x => x.NguoiTao);
            query = query.Include(x => x.NguoiNhan);
            var totalRow = await query.CountAsync();
            var result = await query
            .Skip((bundle.PageIndex - 1) * bundle.PageSize)
            .Take(bundle.PageSize)
            .Select(x => new KeHoachDatHangModel()
            {
                Id = x.Id,
                Ten = x.Ten,
                MaSo = x.MaSo,
                NgayTao = x.NgayTao.ToString("dd/MM/yyyy"),
                NguoiTao = x.NguoiTao.MaSo,
                NguoiNhan = x.NguoiNhan.MaSo,
                NhanKeHoach = x.NhanKeHoach == true ? "Đã nhận kế hoạch" : "Chưa nhận kế hoạch",
                TrangThai = x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh ? "Chưa hoàn thành" :
                                x.TrangThai == TrangThaiKeHoach.HoanThanh ? "Hoàn thành" : "Bị hủy"
            }).ToListAsync();

            var pagedResult = new PagedResult<KeHoachDatHangModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<KeHoachDatHangModel>>(pagedResult);
        }

        public async Task<ApiResult<PagedResult<NhanKeHoachDatHang>>> NhanKeHoachDatHang(GetAllKeHoachDatHang bundle, string userName)
        {
            var index = _context.KeHoachDatHangs;
            IQueryable<KeHoachDatHang> query = index.OrderByDescending(x => x.NgayTao);

            query = query.Include(x => x.NguoiNhan).Where(x => x.NguoiNhan.UserName == userName);
            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(x => x.Ten.Contains(bundle.TuKhoa) || x.MaSo.Contains(bundle.TuKhoa));
            }

            if (!string.IsNullOrEmpty(bundle.NgayDuKien))
            {
                var str = bundle.NgayDuKien.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                query = query.Where(x => x.NgayBatDauDuKien >= ngayBatDauDuKien && x.NgayKetThucDuKien <= ngayKetThucDuKien);
            }

            if (bundle.TrangThai > 0)
            {
                var trangThai = TrangThaiKeHoach.HoanThanh;
                switch (bundle.TrangThai)
                {
                    case 1:
                        trangThai = TrangThaiKeHoach.ChuaHoanThanh;
                        query = query.Where(x => x.TrangThai == trangThai && x.NhanKeHoach == false);
                        break;

                    case 2:
                        trangThai = TrangThaiKeHoach.ChuaHoanThanh;
                        query = query.Where(x => x.TrangThai == trangThai && x.NhanKeHoach == true);
                        break;

                    case 3:
                        trangThai = TrangThaiKeHoach.HoanThanh;
                        query = query.Where(x => x.TrangThai == trangThai);
                        break;

                    case 4:
                        trangThai = TrangThaiKeHoach.BiHuy;
                        query = query.Where(x => x.TrangThai == trangThai);
                        break;
                }
            }

            query = query.Include(x => x.NguoiTao);
            var totalRow = await query.CountAsync();
            var result = await query
            .Skip((bundle.PageIndex - 1) * bundle.PageSize)
            .Take(bundle.PageSize)
            .Select(x => new NhanKeHoachDatHang()
            {
                Id = x.Id,
                Ten = x.Ten,
                MaSo = x.MaSo,
                NgayDuKien = x.NgayBatDauDuKien.ToString("dd/MM/yyyy"),
                NguoiTao = x.NguoiTao.MaSo,
                TrangThai = x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh ? "Chưa hoàn thành" :
                                x.TrangThai == TrangThaiKeHoach.HoanThanh ? "Hoàn thành" : "Bị hủy",
                TinhTrang = x.NhanKeHoach == true ? "Đã nhận" : "Chưa nhận"
            }).ToListAsync();

            var pagedResult = new PagedResult<NhanKeHoachDatHang>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<NhanKeHoachDatHang>>(pagedResult);
        }

        public async Task<ApiResult<bool>> UpdateNhanKeHoach(long id)
        {
            var keHoach = await _context.KeHoachDatHangs.FindAsync(id);
            if (keHoach == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            keHoach.NhanKeHoach = true;
            _context.KeHoachDatHangs.Update(keHoach);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<string>> GetTrangThaiCheBien(long id)
        {
            var datHang = await _context.KeHoachCheBiens.FindAsync(id);
            if (datHang.LenDatHang)
            {
                return new ApiErrorResult<string>("Đã đặt hàng");
            }
            var process = _context.ChiTietCheBiens.Include(x => x.CongThuc)
               .ThenInclude(x => x.ChiTietCongThucs).ThenInclude(x => x.NguyenVatLieu)
               .Where(x => x.IdKeHoachCheBien == id && x.TrangThaiChiTiet == TrangThaiChiTiet.ChuaHoanThanh);

            process = process.Include(x => x.CongThuc.SanPham);

            var result = await process.Select(x => new CongThucModelKeHoach()
            {
                SoLuong = x.SoLuong,
                DonViTinh = x.DonViTinh,
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

            foreach (var item in result)
            {
                foreach (var i in item.DanhSachNguyenVatLieus)
                {
                    var soLuongCT = item.SoLuong * i.SoLuongCanCT;
                    var soLuongNVL = i.SoLuongHienCoNVL / i.GiaTriChuyenDoi;
                    if (soLuongCT > soLuongNVL)
                    {
                        return new ApiErrorResult<string>("Chưa đủ hàng");
                    }
                }
            }
            return new ApiSuccessResult<string>();
        }
    }
}