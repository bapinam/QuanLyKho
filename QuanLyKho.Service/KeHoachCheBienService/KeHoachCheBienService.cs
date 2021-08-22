using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.ThongBaoService;
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

namespace QuanLyKho.Service.KeHoachCheBienService
{
    public class KeHoachCheBienService : IKeHoachCheBienService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IThongBaoService _thongBaoService;

        public KeHoachCheBienService(QuanLyKhoDbContext context, UserManager<AppUser> userManager, IThongBaoService thongBaoService)
        {
            _context = context;
            _userManager = userManager;
            _thongBaoService = thongBaoService;
        }

        public async Task<ApiResult<NhanKetQuaCheBien>> Create(TaoKeHoachCheBien bundle)
        {
            var str = bundle.NgayDuKien.Split(" - ");
            DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
            DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
            // lưu chi tiết chế biến
            List<ChiTietCheBien> chiTietCheBiens = new List<ChiTietCheBien>();
            if (bundle.IdCongThuc == null || bundle.DonViTinh == null || bundle.SoLuong == null)
            {
                return new ApiErrorResult<NhanKetQuaCheBien>("Dữ liệu chi tiết chế biến không được bỏ trống");
            }
            int i = 0;
            foreach (var item in bundle.IdCongThuc)
            {
                chiTietCheBiens.Add(new ChiTietCheBien
                {
                    IdCongThuc = item,
                    DonViTinh = bundle.DonViTinh[i],
                    GhiChu = bundle.GhiChuCheBien[i],
                    SoLuong = bundle.SoLuong[i],
                    TrangThaiChiTiet = TrangThaiChiTiet.ChuaHoanThanh
                });
                i++;
            }
            // tạo mã số
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var viTri = code.ViTri + stt;

            var maSo = code.Ten + viTri;

            var checkCode = await _context.KeHoachCheBiens.AnyAsync(x => x.MaSo == maSo);
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
            var keHoachCheBien = new KeHoachCheBien()
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
                ChiTietCheBiens = chiTietCheBiens
            };
            _context.KeHoachCheBiens.Add(keHoachCheBien);
            await _context.SaveChangesAsync();

            var ngNhan = await _userManager.Users.Where(x => x.Id == bundle.IdNguoiNhan).FirstAsync();
            var result = new NhanKetQuaCheBien()
            {
                Id = keHoachCheBien.Id,
                MaSo = keHoachCheBien.MaSo,
                Ten = keHoachCheBien.Ten,
                NguoiNhan = ngNhan.UserName,
                NgayBatDauDuKien = keHoachCheBien.NgayBatDauDuKien.ToString("yyyy-MM-dd"),
                NgayKetThucDuKien = keHoachCheBien.NgayKetThucDuKien.AddDays(1).ToString("yyyy-MM-dd")
            };

            //Khởi Tạo Thông báo

            var thongBao = new TaoThongBao()
            {
                IdChiMuc = bundle.IdChiMuc,
                DuongDan = "/NhiemVuKeHoachCheBien/Index",
                Ten = "Nhận kế hoạch chế biến: " + result.MaSo,
                NguoiNhan = ngNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanKeHoachCheBien,
                IdNguoiGui = user.Id,
                MaKeHoach = keHoachCheBien.MaSo
            };
            var idThongBao = await _thongBaoService.Create(thongBao);
            result.IdThongBao = idThongBao.ResultObj;

            return new ApiSuccessResult<NhanKetQuaCheBien>(result);
        }

        public async Task<ApiResult<ThongBaoModel>> Delete(long id)
        {
            var keHoachCheBien = await _context.KeHoachCheBiens.Include(x => x.ChiTietCheBiens).FirstAsync(x => x.Id == id);
            if (keHoachCheBien.NhanKeHoach)
            {
                return new ApiErrorResult<ThongBaoModel>("Xóa thật bại vì kế hoạch đã thực thi");
            }
            var maSo = keHoachCheBien.MaSo;
            var chiMuc = keHoachCheBien.IdChiMuc;
            var nguoiNhan = await _userManager.FindByIdAsync(keHoachCheBien.IdNguoiNhan.ToString());
            var maKeHoach = keHoachCheBien.MaSo;
            var nguoiGui = keHoachCheBien.IdNguoiTao;
            _context.ChiTietCheBiens.RemoveRange(keHoachCheBien.ChiTietCheBiens);
            _context.KeHoachCheBiens.Remove(keHoachCheBien);
            await _context.SaveChangesAsync();

            var thongBao = new TaoThongBao()
            {
                IdChiMuc = chiMuc,
                DuongDan = "/NhiemVuKeHoachCheBien/Index",
                Ten = "Xóa kế hoạch chế biến: " + maSo,
                NguoiNhan = nguoiNhan.UserName,
                LoaiThongBao = LoaiThongBao.NhanKeHoachCheBien,
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

        public async Task<List<NhanKetQuaCheBien>> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var keHoachCheBien = _context.KeHoachCheBiens
                               .Where(x => (x.NgayBatDauDuKien.Month == thang && x.NgayBatDauDuKien.Year == nam)
                               || (x.NgayKetThucDuKien.Month == thang && x.NgayKetThucDuKien.Year == nam));
            var result = await keHoachCheBien
                            .Select(x => new NhanKetQuaCheBien()
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

            return new List<NhanKetQuaCheBien>(result);
        }

        public async Task<List<CongThucSanPham>> GetCongThucSanPham(string tuKhoa, int idLoaiSanPham)
        {
            var sanPham = _context.SanPhams.Include(x => x.CongThucs);
            IQueryable<SanPham> sanPhamTK;
            if (!String.IsNullOrEmpty(tuKhoa))
            {
                sanPhamTK = sanPham
                    .Where(x => (x.MaSo.Contains(tuKhoa) || x.Ten.Contains(tuKhoa))
                              && x.IdLoaiSanPham == idLoaiSanPham);
            }
            else
            {
                sanPhamTK = sanPham
                    .Where(x => x.IdLoaiSanPham == idLoaiSanPham);
            }

            var result = await sanPhamTK
                .Select(x => new CongThucSanPham()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    Ten = x.Ten,
                    TrangThaiTonKho = x.NhacNho == true ? (x.SoLuong >= x.MucTonCaoNhat ? true : false) : false,
                    CongThucSanPhamKHs = x.CongThucs.Select(c => new CongThucSanPhamKH()
                    {
                        Id = c.Id,
                        MaSo = c.MaSo,
                        Ten = c.Ten
                    }).ToList()
                }).ToListAsync();

            return new List<CongThucSanPham>(result);
        }

        public async Task<List<DonViTinhModel>> GetDonViTinhKHCB(int idSanPham)
        {
            var donViTinh = await _context.DonViTinhs.Where(x => x.IdSanPham == idSanPham).ToListAsync();
            var result = donViTinh
                .OrderByDescending(x => x.CoBan)
                .Select(x => new DonViTinhModel()
                {
                    Id = x.Id,
                    Ten = x.Ten
                });
            return new List<DonViTinhModel>(result);
        }

        public async Task<List<DonViTinhModel>> GetShowDonViTinhKHCB(int idSanPham)
        {
            var donViTinh = await _context.DonViTinhs.Where(x => x.IdSanPham == idSanPham).ToListAsync();
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

        public async Task<List<GetTenLoaiSP>> GetLoaiSanPhamKH(int id)
        {
            var loaiSanPham = _context.LoaiSanPhams.Where(x => x.IdNhomSanPham == id);
            var result = await loaiSanPham
                .Select(x => new GetTenLoaiSP()
                {
                    Id = x.Id,
                    Ten = x.Ten
                }).ToListAsync();
            return new List<GetTenLoaiSP>(result);
        }

        public async Task<List<GetTenNhomSP>> GetNhomSanPhamKH()
        {
            var loaiSanPham = _context.NhomSanPhams;
            var result = await loaiSanPham
                .Select(x => new GetTenNhomSP()
                {
                    Id = x.Id,
                    Ten = x.Ten
                }).ToListAsync();
            return new List<GetTenNhomSP>(result);
        }

        public async Task<ApiResult<NhanKetQuaCheBien>> Update(CapNhatKeHoachCheBien bundle)
        {
            var keHoachCheBien = await _context.KeHoachCheBiens.Include(x => x.ChiTietCheBiens).FirstAsync(x => x.Id == bundle.Id);
            if (keHoachCheBien.NhanKeHoach)
            {
                return new ApiErrorResult<NhanKetQuaCheBien>("Cập nhật thật bại vì kế hoạch đã thực thi");
            }
            bool giaoKeHoach = false;
            var idNguoiCu = keHoachCheBien.IdNguoiNhan;
            if (keHoachCheBien.IdNguoiNhan != bundle.IdNguoiNhan)
            {
                giaoKeHoach = true;
            }
            var str = bundle.NgayDuKien.Split(" - ");
            DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
            DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);

            keHoachCheBien.Ten = bundle.Ten;
            keHoachCheBien.NgayBatDauDuKien = ngayBatDauDuKien;
            keHoachCheBien.NgayKetThucDuKien = ngayKetThucDuKien;
            keHoachCheBien.IdNguoiNhan = bundle.IdNguoiNhan;
            keHoachCheBien.GhiChu = bundle.GhiChu;

            //cập nhật lại chi tiết chế biến
            List<ChiTietCheBien> chiTietCheBienCapNhats = new List<ChiTietCheBien>();
            if (bundle.IdChiTiet != null && bundle.IdCongThuc != null)
            {
                int i = 0;
                foreach (var item in bundle.IdChiTiet)
                {
                    var chiTiet = await _context.ChiTietCheBiens.FindAsync(item);

                    chiTiet.IdCongThuc = bundle.IdCongThuc[i];
                    chiTiet.SoLuong = bundle.SoLuong[i];
                    chiTiet.GhiChu = bundle.GhiChuCheBien[i];
                    chiTiet.DonViTinh = bundle.DonViTinh[i];
                    chiTietCheBienCapNhats.Add(chiTiet);
                    i++;
                }
                _context.ChiTietCheBiens.UpdateRange(chiTietCheBienCapNhats);
            }

            // thêm mới chi tiết chế biến
            List<ChiTietCheBien> chiTietCheBienThemMois = new List<ChiTietCheBien>();
            if (bundle.IdCongThucThem != null && bundle.SoLuongThem != null && bundle.DonViTinhThem != null)
            {
                int i = 0;
                foreach (var item in bundle.IdCongThucThem)
                {
                    chiTietCheBienThemMois.Add(new ChiTietCheBien
                    {
                        IdCongThuc = item,
                        SoLuong = bundle.SoLuongThem[i],
                        GhiChu = bundle.GhiChuCheBienThem[i],
                        DonViTinh = bundle.DonViTinhThem[i],
                        IdKeHoachCheBien = bundle.Id
                    });
                    i++;
                }
                _context.ChiTietCheBiens.AddRange(chiTietCheBienThemMois);
            }

            _context.KeHoachCheBiens.Update(keHoachCheBien);
            await _context.SaveChangesAsync();

            var result = new NhanKetQuaCheBien()
            {
                Id = keHoachCheBien.Id,
                MaSo = keHoachCheBien.MaSo,
                Ten = keHoachCheBien.Ten,
                NgayBatDauDuKien = keHoachCheBien.NgayBatDauDuKien.ToString("yyyy-MM-dd"),
                NgayKetThucDuKien = keHoachCheBien.NgayKetThucDuKien.AddDays(1).ToString("yyyy-MM-dd")
            };
            result.ThongBaoModelOld = new List<ThongBaoModel>();
            result.ThongBaoModelNew = new List<ThongBaoModel>();
            if (giaoKeHoach)
            {
                var nguoiNhanNew = await _userManager.FindByIdAsync(keHoachCheBien.IdNguoiNhan.ToString());
                var nguoiNhanOld = await _userManager.FindByIdAsync(idNguoiCu.ToString());

                // thông báo người củ kế hoạch đã bị giao
                var thongBaoOld = new TaoThongBao()
                {
                    IdChiMuc = keHoachCheBien.IdChiMuc,
                    DuongDan = "/NhiemVuKeHoachCheBien/Index",
                    Ten = "Đã chuyển kế hoạch chế biến: " + result.MaSo,
                    NguoiNhan = nguoiNhanOld.UserName,
                    LoaiThongBao = LoaiThongBao.NhanKeHoachCheBien,
                    IdNguoiGui = keHoachCheBien.IdNguoiTao,
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
                    IdChiMuc = keHoachCheBien.IdChiMuc,
                    DuongDan = "/NhiemVuKeHoachCheBien/Index",
                    Ten = "Nhận kế hoạch chế biến: " + result.MaSo,
                    NguoiNhan = nguoiNhanNew.UserName,
                    LoaiThongBao = LoaiThongBao.NhanKeHoachCheBien,
                    IdNguoiGui = keHoachCheBien.IdNguoiTao,
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
                var nguoiNhan = await _userManager.FindByIdAsync(keHoachCheBien.IdNguoiNhan.ToString());

                // thông báo cập nhật kế hoạch
                var thongBao = new TaoThongBao()
                {
                    IdChiMuc = keHoachCheBien.IdChiMuc,
                    DuongDan = "/NhiemVuKeHoachCheBien/Index",
                    Ten = "Cập nhật kế hoạch chế biến: " + result.MaSo,
                    NguoiNhan = nguoiNhan.UserName,
                    LoaiThongBao = LoaiThongBao.NhanKeHoachCheBien,
                    IdNguoiGui = keHoachCheBien.IdNguoiTao,
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
            return new ApiSuccessResult<NhanKetQuaCheBien>(result);
        }

        public async Task<List<NhanVienKHCheBien>> NhanVienKHCheBien(string tuKhoa)
        {
            var nhanVien = _userManager.Users.Where(x => x.TinhTrangLamViec == TinhTrangLamViec.DangLamViec
                                                && x.LoaiTaiKhoan == LoaiTaiKhoan.NhanVien);
            if (!String.IsNullOrEmpty(tuKhoa))
            {
                nhanVien = nhanVien.Where(x => x.Ten.Contains(tuKhoa) || x.MaSo.Contains(tuKhoa) || x.CanCuocCongDan.Contains(tuKhoa));
            }
            var result = await nhanVien.Select(x => new NhanVienKHCheBien()
            {
                Id = x.Id,
                CanCuocCongDan = x.CanCuocCongDan,
                MaSo = x.MaSo,
                Ho = x.Ho,
                Ten = x.Ten
            }).ToListAsync();

            return new List<NhanVienKHCheBien>(result);
        }

        public async Task<ApiResult<KeHoachCheBienModel>> GetInfoKeHoachById(long id)
        {
            var keHoach = await _context.KeHoachCheBiens.Include(x => x.ChiTietCheBiens)
                .ThenInclude(c => c.CongThuc)
                .ThenInclude(ct => ct.SanPham)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            var result = new KeHoachCheBienModel()
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
            List<DanhSachCheBien> dsCheBiens = new List<DanhSachCheBien>();
            foreach (var item in keHoach.ChiTietCheBiens)
            {
                var danhSach = new DanhSachCheBien()
                {
                    IdCongThuc = item.IdCongThuc,
                    DonViTinh = item.DonViTinh,
                    IdChiTiet = item.Id,
                    GhiChu = item.GhiChu,
                    SoLuong = item.SoLuong,
                    MaSoCongThuc = item.CongThuc.MaSo,
                    IdSanPham = item.CongThuc.IdSanPham,
                    MaSoSanPham = item.CongThuc.SanPham.MaSo,
                    TenSanPham = item.CongThuc.SanPham.Ten,
                    TrangThai = item.TrangThaiChiTiet == TrangThaiChiTiet.BiHuy ? "Bị hủy" :
                    item.TrangThaiChiTiet == TrangThaiChiTiet.BiHuyMotPhan ? "Bị hủy một phần" :
                    item.TrangThaiChiTiet == TrangThaiChiTiet.DaHoanThanh ? "Đã hoàn thành" : "Chưa hoàn thành"
                };
                dsCheBiens.Add(danhSach);
            }
            result.DanhSachCheBiens = dsCheBiens;
            return new ApiSuccessResult<KeHoachCheBienModel>(result);
        }

        public async Task<ApiResult<bool>> DeleteCongThuc(long id)
        {
            var chiTiet = await _context.ChiTietCheBiens.FindAsync(id);
            if (chiTiet == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            _context.ChiTietCheBiens.Remove(chiTiet);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<PagedResult<KeHoachCheBienModel>>> GetAllKeHoach(GetAllKeHoachCheBien bundle)
        {
            var index = _context.KeHoachCheBiens;
            IQueryable<KeHoachCheBien> query = index.OrderByDescending(x => x.NgayTao);

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
            .Select(x => new KeHoachCheBienModel()
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

            var pagedResult = new PagedResult<KeHoachCheBienModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<KeHoachCheBienModel>>(pagedResult);
        }

        public async Task<ApiResult<PagedResult<NhanKeHoachCheBien>>> NhanKeHoachCheBien(GetAllKeHoachCheBien bundle, string userName)
        {
            var index = _context.KeHoachCheBiens;
            IQueryable<KeHoachCheBien> query = index.OrderByDescending(x => x.NgayTao);

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
            .Select(x => new NhanKeHoachCheBien()
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

            var pagedResult = new PagedResult<NhanKeHoachCheBien>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<NhanKeHoachCheBien>>(pagedResult);
        }

        public async Task<ApiResult<bool>> UpdateNhanKeHoach(long id)
        {
            var keHoach = await _context.KeHoachCheBiens.FindAsync(id);
            if (keHoach == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            keHoach.NhanKeHoach = true;
            _context.KeHoachCheBiens.Update(keHoach);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}