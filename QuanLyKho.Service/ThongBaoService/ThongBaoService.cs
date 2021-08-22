using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Date.EF;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.ThongBaoService
{
    public class ThongBaoService : IThongBaoService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ThongBaoService(QuanLyKhoDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApiResult<long>> Create(TaoThongBao bundle)
        {
            var idNguoiNhan = await _userManager.FindByNameAsync(bundle.NguoiNhan);
            var thongBao = new ThongBao()
            {
                IdChiMuc = bundle.IdChiMuc,
                DuongDan = bundle.DuongDan,
                Ten = bundle.Ten,
                NgayNhan = DateTime.Now,
                Xem = false,
                IdNguoiNhan = idNguoiNhan.Id,
                MaKeHoach = bundle.MaKeHoach,
                LoaiThongBao = bundle.LoaiThongBao,
                IdNguoiGui = bundle.IdNguoiGui
            };

            _context.ThongBaos.Add(thongBao);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<long>(thongBao.Id);
        }

        public async Task<ApiResult<bool>> UpdateDaXem(long id)
        {
            var thongBao = await _context.ThongBaos.FindAsync(id);
            if (thongBao == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            thongBao.Xem = true;
            _context.ThongBaos.Update(thongBao);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(long id)
        {
            var thongBao = await _context.ThongBaos.FindAsync(id);
            if (thongBao == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            _context.ThongBaos.Remove(thongBao);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<PagedResult<ThongBaoModel>>> GetThongBaoPaging(SetThongBao bundle)
        {
            var index = _context.ThongBaos;
            IQueryable<ThongBao> query = index.OrderByDescending(x => x.NgayNhan).OrderBy(x => x.Xem);

            query = query.Include(x => x.NguoiNhan).Where(x => x.NguoiNhan.UserName == bundle.UserName);
            query = query.Include(x => x.NguoiGui);

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(x => x.Ten.Contains(bundle.TuKhoa));
            }

            if (!string.IsNullOrEmpty(bundle.NgayNhanThongBao))
            {
                var str = bundle.NgayNhanThongBao.Split(" - ");
                DateTime ngayBatDau = Convert.ToDateTime(str[0]);
                DateTime ngayKetThuc = Convert.ToDateTime(str[1]);
                query = query.Where(x => x.NgayNhan.Date >= ngayBatDau.Date && x.NgayNhan.Date <= ngayKetThuc.Date);
            }

            if (bundle.TrangThaiXem > 0 && bundle.TrangThaiXem < 3)
            {
                switch (bundle.TrangThaiXem)
                {
                    case 1:
                        query = query.Where(x => x.Xem == false);
                        break;

                    case 2:
                        query = query.Where(x => x.Xem == true);
                        break;
                }
            }
            var totalRow = await query.CountAsync();
            var result = await query
            .Skip((bundle.PageIndex - 1) * bundle.PageSize)
            .Take(bundle.PageSize)
            .Select(x => new ThongBaoModel()
            {
                Id = x.Id,
                Ten = x.Ten,
                SoNgayNhan = (DateTime.Now.Date - x.NgayNhan.Date).Days,
                NgayNhan = x.NgayNhan.ToString("dd/MM/yyyy"),
                Xem = x.Xem == true ? "Đã xem" : "Chưa xem",
                NguoiGui = x.NguoiGui.MaSo,
                DuongDan = x.DuongDan
            }).ToListAsync();

            var pagedResult = new PagedResult<ThongBaoModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<ThongBaoModel>>(pagedResult);
        }

        public async Task<List<ThongBaoModel>> GetTopThongBao(string userName)
        {
            var thongBao = _context.ThongBaos.Include(x => x.NguoiNhan);
            var nguoiNhan = thongBao.Where(x => x.NguoiNhan.UserName == userName);
            var ketQua = await nguoiNhan
                .OrderByDescending(x => x.NgayNhan)
                .OrderBy(x => x.Xem)
                .Take(10)
                .Select(x => new ThongBaoModel()
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    SoNgayNhan = (DateTime.Now.Date - x.NgayNhan.Date).Days,
                    DuongDan = x.DuongDan,
                    Xem = x.Xem == true ? "Đã xem" : "Chưa xem"
                }).ToListAsync();
            return new List<ThongBaoModel>(ketQua);
        }

        public async Task<ApiResult<string>> DeleteAll()
        {
            DateTime ngayHienTai = DateTime.Now;
            var listDay = _context.ThongBaos.OrderByDescending(x => x.NgayNhan);

            var result = await listDay.ToListAsync();

            if (result.Count == 0)
            {
                return new ApiErrorResult<string>("Chua co thong bao vuot qua 30 ngay");
            }

            int soLuong = 0;
            List<ThongBao> thongBaos = new List<ThongBao>();
            foreach (var item in result)
            {
                var soNgay = (ngayHienTai.Date - item.NgayNhan.Date).Days;
                if (soNgay > 30)
                {
                    thongBaos.Add(item);
                    soLuong++;
                }
            }
            _context.RemoveRange(thongBaos);
            await _context.SaveChangesAsync();
            var str = "Da xoa " + soLuong + " thong bao!!";
            return new ApiSuccessResult<string>(str);
        }
    }
}