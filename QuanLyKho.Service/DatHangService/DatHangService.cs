using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DatHang;
using QuanLyKho.ViewModels.KeHoachDatHang;
using QuanLyKho.ViewModels.NhaCungCap;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.DatHangService
{
    public class DatHangService : IDatHangService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IThongBaoService _thongBaoService;

        public DatHangService(QuanLyKhoDbContext context, UserManager<AppUser> userManager,
            IThongBaoService thongBaoService)
        {
            _context = context;
            _userManager = userManager;
            _thongBaoService = thongBaoService;
        }

        public async Task<ApiResult<PhieuNhapModel>> Create(NhapHoaDon bundle)
        {
            if (bundle.IdChiTiet == null || bundle.IdChiTiet.Length == 0)
            {
                return new ApiErrorResult<PhieuNhapModel>("Vui lòng nhập nguyên vật liệu bất ký");
            }

            // nhập chi tiết hóa đơn
            List<ChiTietHoaDon> chiTietHoaDons = new List<ChiTietHoaDon>();
            List<NguyenVatLieu> nguyenVatLieus = new List<NguyenVatLieu>();
            List<ChiTietDatHang> chiTietDatHangs = new List<ChiTietDatHang>();
            List<NhacNhoSoLuong> nhacNhoSoLuongs = new List<NhacNhoSoLuong>();

            var idNguoiTao = await _userManager.FindByNameAsync(bundle.NguoiTao);

            var i = 0;

            foreach (var item in bundle.IdChiTiet)
            {
                if (bundle.SoLuongDaThem[i] != 0)
                {
                    var tongTien = bundle.DonGia[i] * bundle.SoLuongDaThem[i];
                    double giamGiaTien = 0;
                    double thanhTien = 0;
                    if (bundle.GiamGia[i] == 0)
                    {
                        thanhTien = tongTien;
                    }
                    else
                    {
                        giamGiaTien = tongTien * bundle.GiamGia[i] / 100;
                        thanhTien = tongTien - giamGiaTien;
                    }

                    var chiTiet = new ChiTietHoaDon()
                    {
                        IdNguyenVatLieu = bundle.IdNguyenVatLieu[i],
                        DonViTinh = bundle.DonViTinh[i],
                        DonGia = (decimal)bundle.DonGia[i],
                        GiamGia = bundle.GiamGia[i],
                        SoLuong = bundle.SoLuongDaThem[i],
                        ThanhTien = (decimal)thanhTien
                    };

                    chiTietHoaDons.Add(chiTiet);

                    // lưu lại nguyên vật liệu vào kho
                    var donViTinhNhap = bundle.DonViTinh[i];
                    var soLuongNhap = bundle.SoLuongDaThem[i];
                    var giaTriChuyenDoi = await _context.DonViTinhs
                        .Where(x => x.IdNguyenVatLieu == bundle.IdNguyenVatLieu[i]
                        && x.Ten == donViTinhNhap).Select(x => x.GiaTriChuyenDoi).FirstAsync();

                    var soLuongCanThemNVL = soLuongNhap * giaTriChuyenDoi;

                    var nvl = await _context.NguyenVatLieus.FindAsync(bundle.IdNguyenVatLieu[i]);
                    var soLuongCu = nvl.SoLuong;
                    var soLuongThemMoi = soLuongCu + soLuongCanThemNVL;
                    nvl.SoLuong = soLuongThemMoi;
                    nguyenVatLieus.Add(nvl);

                    // lưu lại chi tiết đặt hàng
                    int soLuongThemHienCo = 0;
                    var chiTietDH = await _context.ChiTietDatHangs.FindAsync(item);
                    soLuongThemHienCo = chiTietDH.SoLuongDaThem;
                    var soLuongThemMoiNhat = soLuongThemHienCo + bundle.SoLuongDaThem[i];
                    chiTietDH.SoLuongDaThem = soLuongThemMoiNhat;
                    if (soLuongThemMoiNhat >= bundle.SoLuong[i])
                    {
                        chiTietDH.TrangThaiChiTiet = TrangThaiChiTiet.DaHoanThanh;
                    }
                    chiTietDatHangs.Add(chiTietDH);

                    // kiểm tra số Lượng có vượt mức tồn hay không?
                    if (nvl.NhacNho)
                    {
                        if (soLuongThemMoi >= nvl.MucTonCaoNhat)
                        {
                            var vaiTro = await _userManager.GetUsersInRoleAsync("QuanLyDatHang");

                            foreach (var vt in vaiTro)
                            {
                                if (vt.TinhTrangLamViec == TinhTrangLamViec.DangLamViec)
                                {
                                    var ngNhanTB = vt.UserName;
                                    // tạo thông báo
                                    var thongBaoUser = new TaoThongBao()
                                    {
                                        IdChiMuc = bundle.IdChiMuc,
                                        DuongDan = "/CheBiens/Index",
                                        Ten = "Nguyên vật liệu " + nvl.MaSo + " vượt quá tồn",
                                        NguoiNhan = ngNhanTB,
                                        LoaiThongBao = LoaiThongBao.NhacNho,
                                        IdNguoiGui = idNguoiTao.Id,
                                        MaKeHoach = nvl.Ten
                                    };
                                    var idThongBaoUser = await _thongBaoService.Create(thongBaoUser);

                                    var nhacNhoUser = new NhacNhoSoLuong()
                                    {
                                        DuongDan = "/CheBiens/Index",
                                        Ten = "Nguyên vật liệu " + nvl.MaSo + " vượt quá tồn",
                                        IdThongBao = idThongBaoUser.ResultObj,
                                        NguoiNhan = ngNhanTB
                                    };
                                    nhacNhoSoLuongs.Add(nhacNhoUser);
                                }
                            }

                            // tạo thông báo
                            var thongBao = new TaoThongBao()
                            {
                                IdChiMuc = bundle.IdChiMuc,
                                DuongDan = "/CheBiens/Index",
                                Ten = "Nguyên vật liệu " + nvl.MaSo + " vượt quá tồn",
                                NguoiNhan = "admin",
                                LoaiThongBao = LoaiThongBao.NhacNho,
                                IdNguoiGui = idNguoiTao.Id,
                                MaKeHoach = nvl.Ten
                            };
                            var idThongBao = await _thongBaoService.Create(thongBao);

                            var nhacNho = new NhacNhoSoLuong()
                            {
                                DuongDan = "/CheBiens/Index",
                                Ten = "Nguyên vật liệu " + nvl.MaSo + " vượt quá tồn",
                                IdThongBao = idThongBao.ResultObj,
                                NguoiNhan = "admin"
                            };
                            nhacNhoSoLuongs.Add(nhacNho);
                        }
                    }
                }
                i++;
            }
            // tạo code
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaLuuTru);
            var stt = 1;
            ViTri:
            var location = code.ViTri + stt;

            var maLuu = code.Ten + location;

            var checkCode = await _context.HoaDons.AnyAsync(x => x.MaLuuTru == maLuu);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = location;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();
            decimal tienDaTra = Decimal.Parse(bundle.SoTienDaThanhToan);
            decimal tongTienThanhToan = (decimal)bundle.TongTien;

            ThanhToanHoaDon ketQuaThanhToan = ThanhToanHoaDon.ChuaThanhToan;
            if (tienDaTra >= tongTienThanhToan)
            {
                ketQuaThanhToan = ThanhToanHoaDon.DaThanhToan;
            }

            if (tienDaTra > 0 && tienDaTra < tongTienThanhToan)
            {
                ketQuaThanhToan = ThanhToanHoaDon.ThanhToanMotPhan;
            }

            var hoaDon = new HoaDon()
            {
                IdChiMuc = bundle.IdChiMuc,
                IdKeHoach = bundle.IdKeHoach,
                IdNCC = bundle.IdNhaCungCap,
                MaLuuTru = maLuu,
                IdNguoiTao = idNguoiTao.Id,
                NgayMua = bundle.NgayMua,
                NgayTao = DateTime.Now,
                SoHoaDon = bundle.SoHoaDon,
                SoTienDaTra = tienDaTra,
                ThueSuat = bundle.SoThue,
                ThanhToanHoaDon = ketQuaThanhToan,
                TongTien = tongTienThanhToan,
                ChiTietHoaDons = chiTietHoaDons
            };

            _context.HoaDons.Add(hoaDon);
            _context.NguyenVatLieus.UpdateRange(nguyenVatLieus);
            _context.ChiTietDatHangs.UpdateRange(chiTietDatHangs);
            await _context.SaveChangesAsync();

            // cập nhật lại kế hoạch đặt hàng
            var khDH = await _context.KeHoachDatHangs.
                Include(x => x.ChiTietDatHangs).Where(x => x.Id == bundle.IdKeHoach).FirstAsync();

            bool giaTri = false;
            foreach (var item in khDH.ChiTietDatHangs)
            {
                if (item.TrangThaiChiTiet != TrangThaiChiTiet.ChuaHoanThanh)
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
                khDH.TrangThai = TrangThaiKeHoach.HoanThanh;
                _context.KeHoachDatHangs.Update(khDH);
                await _context.SaveChangesAsync();
            }
            var ketQuaPhieu = new PhieuNhapModel()
            {
                Id = hoaDon.Id,
                IdKeHoach = khDH.Id,
                MaLuuTru = hoaDon.MaLuuTru,
                NgayTao = hoaDon.NgayTao.ToString("dd-MM-yyyy"),
                TrangThai = hoaDon.ThanhToanHoaDon == ThanhToanHoaDon.ChuaThanhToan ? "Chưa thanh toán" :
                      hoaDon.ThanhToanHoaDon == ThanhToanHoaDon.ThanhToanMotPhan ? "Thanh toán một phần" : "Đã thanh toán",
                MaKeHoach = khDH.MaSo,
                NguoiTao = idNguoiTao.MaSo,
                TrangThaiHoanKH = khDH.TrangThai != TrangThaiKeHoach.ChuaHoanThanh ? "1" : "0",
                NhacNhoSoLuongs = nhacNhoSoLuongs
            };

            return new ApiSuccessResult<PhieuNhapModel>(ketQuaPhieu);
        }

        public async Task<List<KeHoachDatHangModel>> GetAllKeHoachDatHangChuaHoanThanh(TimKiemDatHang bundle)
        {
            var datHang = _context.KeHoachDatHangs.Where(x => x.NhanKeHoach == true && x.TrangThai == TrangThaiKeHoach.ChuaHoanThanh);

            if (!String.IsNullOrEmpty(bundle.TuKhoa))
            {
                datHang = datHang.Where(x => x.MaSo.Contains(bundle.TuKhoa) || x.Ten.Contains(bundle.TuKhoa));
            }
            if (!String.IsNullOrEmpty(bundle.Ngay))
            {
                var str = bundle.Ngay.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                datHang = datHang.Where(x => x.NgayBatDauDuKien.Date >= ngayBatDauDuKien.Date
                && x.NgayKetThucDuKien.Date <= ngayKetThucDuKien.Date);
            }
            datHang = datHang.Include(x => x.NguoiTao);
            datHang = datHang.Include(x => x.NguoiNhan);
            var result = await datHang
                .Select(x => new KeHoachDatHangModel
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

            return new List<KeHoachDatHangModel>(result);
        }

        public async Task<List<PhieuNhapModel>> GetAllPhieuNhapChuaTra(TimKiemDatHang bundle)
        {
            var phieu = _context.HoaDons.Include(x => x.KeHoachDatHang).Include(x => x.NguoiTao)
                .Where(x => x.ThanhToanHoaDon != ThanhToanHoaDon.DaThanhToan);
            if (!String.IsNullOrEmpty(bundle.TuKhoa))
            {
                phieu = phieu.Where(x => x.MaLuuTru.Contains(bundle.TuKhoa) || x.KeHoachDatHang.MaSo.Contains(bundle.TuKhoa));
            }
            if (!String.IsNullOrEmpty(bundle.Ngay))
            {
                var str = bundle.Ngay.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                phieu = phieu.Where(x => x.NgayTao.Date >= ngayBatDauDuKien.Date && x.NgayTao.Date <= ngayKetThucDuKien.Date);
            }
            var result = await phieu
                  .Select(x => new PhieuNhapModel()
                  {
                      Id = x.Id,
                      MaLuuTru = x.MaLuuTru,
                      MaKeHoach = x.KeHoachDatHang.MaSo,
                      NgayTao = x.NgayTao.ToString("dd/MM/yyyy"),
                      NguoiTao = x.NguoiTao.MaSo,
                      TrangThai = x.ThanhToanHoaDon == ThanhToanHoaDon.ChuaThanhToan ? "Chưa thanh toán" :
                      x.ThanhToanHoaDon == ThanhToanHoaDon.ThanhToanMotPhan ? "Thanh toán một phần" : "Đã thanh toán"
                  }).ToListAsync();
            return new List<PhieuNhapModel>(result);
        }

        public async Task<ApiResult<PagedResult<PhieuNhapModel>>> GetAllPhieuNhapDaTra(TimKiemPhieuNhapDaTra bundle)
        {
            var phieu = _context.HoaDons.Include(x => x.KeHoachDatHang).Include(x => x.NguoiTao)
                .Where(x => x.ThanhToanHoaDon == ThanhToanHoaDon.DaThanhToan);
            if (!String.IsNullOrEmpty(bundle.TuKhoa))
            {
                phieu = phieu.Where(x => x.MaLuuTru.Contains(bundle.TuKhoa) || x.KeHoachDatHang.MaSo.Contains(bundle.TuKhoa));
            }
            if (!String.IsNullOrEmpty(bundle.Ngay))
            {
                var str = bundle.Ngay.Split(" - ");
                DateTime ngayBatDauDuKien = Convert.ToDateTime(str[0]);
                DateTime ngayKetThucDuKien = Convert.ToDateTime(str[1]);
                phieu = phieu.Where(x => x.NgayTao.Date >= ngayBatDauDuKien.Date && x.NgayTao.Date <= ngayKetThucDuKien.Date);
            }

            var totalRow = await phieu.CountAsync();

            var result = await phieu
            .Skip((bundle.PageIndex - 1) * bundle.PageSize)
            .Take(bundle.PageSize)
             .Select(x => new PhieuNhapModel
             {
                 Id = x.Id,
                 MaLuuTru = x.MaLuuTru,
                 MaKeHoach = x.KeHoachDatHang.MaSo,
                 NgayTao = x.NgayTao.ToString("dd/MM/yyyy"),
                 NguoiTao = x.NguoiTao.MaSo,
                 TrangThai = x.ThanhToanHoaDon == ThanhToanHoaDon.ChuaThanhToan ? "Chưa thanh toán" :
                      x.ThanhToanHoaDon == ThanhToanHoaDon.ThanhToanMotPhan ? "Thanh toán một phần" : "Đã thanh toán"
             }).ToListAsync();

            var pagedResult = new PagedResult<PhieuNhapModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<PhieuNhapModel>>(pagedResult);
        }

        public async Task<ApiResult<ChiTietPhieuNhapModel>> GetChiTietPhieuNhap(long id)
        {
            var chiTiet = await _context.HoaDons.Include(x => x.ChiTietHoaDons)
                .ThenInclude(x => x.NguyenVatLieu)
                .Include(x => x.KeHoachDatHang)
                .Include(x => x.NhaCungCap).Include(x => x.NguoiTao)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            if (chiTiet == null)
            {
                return new ApiErrorResult<ChiTietPhieuNhapModel>("Id không tồn tại");
            }
            var result = new ChiTietPhieuNhapModel()
            {
                Id = chiTiet.Id,
                MaKeHoach = chiTiet.KeHoachDatHang.MaSo,
                MaLuuTru = chiTiet.MaLuuTru,
                MaNhaCungCap = chiTiet.NhaCungCap.MaSo,
                NgayMua = chiTiet.NgayMua.ToString("dd-MM-yyyy"),
                NgayTao = chiTiet.NgayTao.ToString("dd-MM-yyyy"),
                NguoiTao = chiTiet.NguoiTao.MaSo,
                ThueSuat = chiTiet.ThueSuat,
                SoHoaDon = chiTiet.SoHoaDon,
                TrangThaiTraTien = chiTiet.ThanhToanHoaDon == ThanhToanHoaDon.ChuaThanhToan ? "Chưa thanh toán" :
                      chiTiet.ThanhToanHoaDon == ThanhToanHoaDon.ThanhToanMotPhan ? "Thanh toán một phần" : "Đã thanh toán",
                TongTienThanhToan = chiTiet.TongTien,
                DanhSachNguyenLieuNhapHDs = chiTiet.ChiTietHoaDons
                .Select(x => new DanhSachNguyenLieuNhapHD()
                {
                    IdChiTiet = x.Id,
                    DonViTinh = x.DonViTinh,
                    GiamGia = x.GiamGia,
                    SoLuong = x.SoLuong,
                    TenNguyenVatLieu = x.NguyenVatLieu.Ten,
                    MaNguyenVatLieu = x.NguyenVatLieu.MaSo,
                    DonGia = x.DonGia
                }).ToList()
            };
            return new ApiSuccessResult<ChiTietPhieuNhapModel>(result);
        }

        public async Task<List<DanhSachNguyenLieuDatHang>> GetDanhSachDatHang(long id)
        {
            var keHoach = await _context.KeHoachDatHangs.Include(x => x.ChiTietDatHangs)
                .ThenInclude(x => x.NguyenVatLieu).ThenInclude(x => x.DonViTinhs)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            var result = keHoach.ChiTietDatHangs
                .Where(x => x.TrangThaiChiTiet == TrangThaiChiTiet.ChuaHoanThanh)
                .Select(x => new DanhSachNguyenLieuDatHang()
                {
                    IdNguyenVatLieu = x.IdNguyenVatLieu,
                    IdChiTiet = x.Id,
                    MaSo = x.NguyenVatLieu.MaSo,
                    Ten = x.NguyenVatLieu.Ten,
                    DonViTinh = x.DonViTinh,
                    SoLuong = x.SoLuong,
                    SoLuongDaThem = x.SoLuongDaThem
                });

            return new List<DanhSachNguyenLieuDatHang>(result);
        }

        public async Task<ApiResult<string>> HuyChiTietDatHang(long id)
        {
            var datHang = await _context.ChiTietDatHangs.FindAsync(id);
            if (datHang == null)
            {
                return new ApiErrorResult<string>("Id không tồn tại");
            }
            datHang.TrangThaiChiTiet = TrangThaiChiTiet.BiHuy;

            if (datHang.SoLuongDaThem > 0)
            {
                datHang.TrangThaiChiTiet = TrangThaiChiTiet.BiHuyMotPhan;
            }
            _context.ChiTietDatHangs.Update(datHang);
            await _context.SaveChangesAsync();

            // xem chi tiết có cái nào chưa đặt hàng ko
            var keHoach = await _context.KeHoachDatHangs.Include(x => x.ChiTietDatHangs)
                .Where(x => x.Id == datHang.IdKeHoachDatHang).FirstAsync();
            bool giaTri = false;
            foreach (var kiemTra in keHoach.ChiTietDatHangs)
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
            string phanHoi = "false";
            if (giaTri)
            {
                keHoach.TrangThai = TrangThaiKeHoach.HoanThanh;
                _context.KeHoachDatHangs.Update(keHoach);
                await _context.SaveChangesAsync();
                phanHoi = "true";
            }
            return new ApiSuccessResult<string>(phanHoi);
        }

        public async Task<List<NhaCungCapModel>> NhaCungCapDatHang(string tuKhoa)
        {
            var ncc = await _context.NhaCungCaps
                .Where(x => x.Ten.Contains(tuKhoa) || x.MaSo.Contains(tuKhoa))
                .Select(x => new NhaCungCapModel()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    Ten = x.Ten,
                    DiaChi = x.DiaChi
                }).ToListAsync();

            return new List<NhaCungCapModel>(ncc);
        }

        public async Task<ApiResult<PhieuNhapModel>> UpdateThanhToan(long id, string tienDaTra)
        {
            var hoaDon = await _context.HoaDons.Include(x => x.KeHoachDatHang)
                .Include(x => x.NguoiTao).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (hoaDon == null)
            {
                return new ApiErrorResult<PhieuNhapModel>("Không tồn tại id");
            }
            decimal soTien = Decimal.Parse(tienDaTra);
            var tongTienThanhToan = hoaDon.TongTien;
            ThanhToanHoaDon ketQuaThanhToan = ThanhToanHoaDon.ChuaThanhToan;
            if (soTien >= tongTienThanhToan)
            {
                ketQuaThanhToan = ThanhToanHoaDon.DaThanhToan;
            }

            if (soTien > 0 && soTien < tongTienThanhToan)
            {
                ketQuaThanhToan = ThanhToanHoaDon.ThanhToanMotPhan;
            }
            hoaDon.SoTienDaTra = soTien;
            hoaDon.ThanhToanHoaDon = ketQuaThanhToan;
            _context.HoaDons.Update(hoaDon);
            await _context.SaveChangesAsync();
            var ketQuaPhieu = new PhieuNhapModel()
            {
                Id = hoaDon.Id,
                MaLuuTru = hoaDon.MaLuuTru,
                NgayTao = hoaDon.NgayTao.ToString("dd-MM-yyyy"),
                TrangThai = hoaDon.ThanhToanHoaDon == ThanhToanHoaDon.ChuaThanhToan ? "Chưa thanh toán" :
                    hoaDon.ThanhToanHoaDon == ThanhToanHoaDon.ThanhToanMotPhan ? "Thanh toán một phần" : "Đã thanh toán",
                MaKeHoach = hoaDon.KeHoachDatHang.MaSo,
                NguoiTao = hoaDon.NguoiTao.MaSo
            };
            return new ApiSuccessResult<PhieuNhapModel>(ketQuaPhieu);
        }

        public async Task<ApiResult<TienDaThanhToan>> GetTienDaThanhToan(long id)
        {
            var tien = await _context.HoaDons.FindAsync(id);
            if (tien == null)
            {
                return new ApiErrorResult<TienDaThanhToan>("Không tồn tại id");
            }

            var ketQuaPhieu = new TienDaThanhToan()
            {
                TienCanThanhToan = tien.TongTien,
                TienDaTra = tien.SoTienDaTra
            };
            return new ApiSuccessResult<TienDaThanhToan>(ketQuaPhieu);
        }
    }
}