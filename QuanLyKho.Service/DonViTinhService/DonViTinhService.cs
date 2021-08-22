using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.DonViTinhService
{
    public class DonViTinhService : IDonViTinhService
    {
        private readonly QuanLyKhoDbContext _context;

        public DonViTinhService(QuanLyKhoDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<DonViTinhModel>> Create(TaoDonViTinh bundle)
        {
            var donViTinh = new DonViTinh()
            {
                Ten = bundle.Ten,
                GiaTriChuyenDoi = bundle.GiaTriChuyenDoi,
                CoBan = false,
            };
            if (bundle.LoaiDongGoi == LoaiDongGoi.NguyenVatLieu)
            {
                donViTinh.IdNguyenVatLieu = bundle.IdThem;
            }
            if (bundle.LoaiDongGoi == LoaiDongGoi.SanPham)
            {
                donViTinh.IdSanPham = bundle.IdThem;
            }
            _context.DonViTinhs.Add(donViTinh);
            await _context.SaveChangesAsync();

            var result = new DonViTinhModel()
            {
                Ten = donViTinh.Ten,
                GiaTriChuyenDoi = donViTinh.GiaTriChuyenDoi,
                Id = donViTinh.Id
            };
            return new ApiSuccessResult<DonViTinhModel>(result);
        }

        public async Task<ApiResult<bool>> Delete(long id)
        {
            var donViTinh = await _context.DonViTinhs.FindAsync(id);
            if (donViTinh == null)
            {
                return new ApiErrorResult<bool>("Đơn vị tínhkhông tồn tại");
            }

            var reult = _context.DonViTinhs.Remove(donViTinh);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<DonViTinhModel>> GetDonViTinh(int id, LoaiDongGoi loaiDongGoi)
        {
            var donViTinh = _context.DonViTinhs;

            IQueryable<DonViTinh> danhSachDVT = null;

            if (loaiDongGoi == LoaiDongGoi.NguyenVatLieu)
            {
                danhSachDVT = donViTinh.Include(x => x.NguyenVatLieu).Where(x => x.IdNguyenVatLieu == id);
            }

            if (loaiDongGoi == LoaiDongGoi.SanPham)
            {
                danhSachDVT = donViTinh.Include(x => x.SanPham).Where(x => x.IdSanPham == id);
            }

            var result = await danhSachDVT.Select(
                i => new DonViTinhModel()
                {
                    Id = i.Id,
                    Ten = i.Ten,
                    GiaTriChuyenDoi = i.GiaTriChuyenDoi,
                    CoBan = i.CoBan,
                    IdNVL = i.IdNguyenVatLieu,
                    IdSP = i.IdSanPham
                }
                ).ToListAsync();

            return new List<DonViTinhModel>(result);
        }

        public async Task<ApiResult<bool>> iTen(string ten, int id, long? idDVT, bool loaiDongGoi)
        {
            var donViTinh = _context.DonViTinhs;
            bool ketQua = false;
            if (idDVT != null)
            {
                if (!loaiDongGoi)
                {
                    ketQua = await donViTinh.AnyAsync(x => x.IdNguyenVatLieu == id && x.Id != idDVT &&
                    EF.Functions.Collate(x.Ten.ToUpper().Trim(), SystemConstants.Collate_AS) == ten.ToUpper().Trim());
                }

                if (loaiDongGoi)
                {
                    ketQua = await donViTinh.AnyAsync(x => x.IdSanPham == id && x.Id != idDVT &&
                    EF.Functions.Collate(x.Ten.ToUpper().Trim(), SystemConstants.Collate_AS) == ten.ToUpper().Trim());
                }
            }
            else
            {
                if (!loaiDongGoi)
                {
                    ketQua = await donViTinh.AnyAsync(x => x.IdNguyenVatLieu == id &&
                    EF.Functions.Collate(x.Ten.ToUpper().Trim(), SystemConstants.Collate_AS) == ten.ToUpper().Trim());
                }

                if (loaiDongGoi)
                {
                    ketQua = await donViTinh.AnyAsync(x => x.IdSanPham == id &&
                    EF.Functions.Collate(x.Ten.ToUpper().Trim(), SystemConstants.Collate_AS) == ten.ToUpper().Trim());
                }
            }

            if (ketQua)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Không tồn tại tên");
        }

        public async Task<ApiResult<bool>> Update(CapNhatDonViTinh bundle)
        {
            var donViTinh = await _context.DonViTinhs.FindAsync(bundle.Id);
            if (donViTinh == null)
            {
                return new ApiErrorResult<bool>("Đơn vị tínhkhông tồn tại");
            }

            donViTinh.Ten = bundle.Ten;
            donViTinh.GiaTriChuyenDoi = bundle.GiaTriChuyenDoi;

            _context.DonViTinhs.Update(donViTinh);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }
    }
}