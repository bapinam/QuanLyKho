using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.Common;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NguyenVatLieu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.NguyenVatLieuService
{
    public class NguyenVatLieuService : INguyenVatLieuService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly IFileStorageService _storageService;
        private const string PRODUCT_CONTENT_FOLDER_NAME = SystemConstants.ImageFolder;

        public NguyenVatLieuService(QuanLyKhoDbContext context, IFileStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> CapNhatHinhAnh(int id, CapNhatHinhAnhNVL bundle)
        {
            var nguyenVatLieu = await _context.NguyenVatLieus.FindAsync(id);
            if (nguyenVatLieu == null)
            {
                return new ApiErrorResult<string>("Nguyên vật liệu không tồn tại");
            }
            // xóa ảnh củ
            if (nguyenVatLieu.HinhAnh != null)
            {
                await _storageService.DeleteFileAsync(nguyenVatLieu.HinhAnh);
            }

            // lưu lại ảnh mới
            nguyenVatLieu.HinhAnh = await this.SaveFile(bundle.HinhAnh);
            _context.NguyenVatLieus.Update(nguyenVatLieu);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<string>(nguyenVatLieu.HinhAnh);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + PRODUCT_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<ApiResult<NguyenVatLieuModel>> Create(TaoNguyenVatLieu bundle)
        {
            // thêm đơn vị tính
            var pack = new List<DonViTinh>();
            pack.Add(new DonViTinh()
            {
                Ten = bundle.TenDVTCoBan,
                GiaTriChuyenDoi = 1,
                LoaiDongGoi = LoaiDongGoi.NguyenVatLieu,
                CoBan = true
            });

            if (bundle.TenDVT != null)
            {
                int i = 0;
                foreach (string item in bundle.TenDVT)
                {
                    pack.Add(new DonViTinh()
                    {
                        Ten = item,
                        GiaTriChuyenDoi = (long)bundle.GiaTriDVT[i],
                        LoaiDongGoi = LoaiDongGoi.NguyenVatLieu,
                        CoBan = false
                    });
                    i++;
                }
            }

            // thêm nguyên vật liệu
            var nguyenVatLieu = new NguyenVatLieu()
            {
                MaSo = bundle.MaSo,
                Ten = bundle.Ten,
                MoTa = bundle.MoTa,
                IdLoaiNVL = bundle.IdLoaiNguyenVatLieu,
                DonViTinhs = pack,
                NhacNho = bundle.NhacNho,
                MucTonThapNhat = (long)bundle.MucTonThapNhat,
                MucTonCaoNhat = bundle.MucTonCaoNhat
            };

            // mã số
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var viTri = code.ViTri + stt;

            var str = code.Ten + viTri;

            var checkCode = await _context.NguyenVatLieus.AnyAsync(x => x.MaSo == str);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = viTri;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            nguyenVatLieu.MaSo = str;
            _context.NguyenVatLieus.Add(nguyenVatLieu);
            await _context.SaveChangesAsync();

            // trả về kết quả sau khi lưu thành công
            var ketQua = new NguyenVatLieuModel()
            {
                Id = nguyenVatLieu.Id,
                MaSo = nguyenVatLieu.MaSo,
                Ten = nguyenVatLieu.Ten,
                SoLuong = nguyenVatLieu.SoLuong,
                NhacNho = nguyenVatLieu.NhacNho,
                TenDVTCoBan = bundle.TenDVTCoBan,
                MucTonCaoNhat = nguyenVatLieu.MucTonCaoNhat,
                MucTonThapNhat = nguyenVatLieu.MucTonThapNhat
            };
            var loaiNVL = await _context.LoaiNguyenVatLieus.FindAsync(nguyenVatLieu.IdLoaiNVL);
            ketQua.TenLoaiNVL = loaiNVL.Ten;

            return new ApiSuccessResult<NguyenVatLieuModel>(ketQua);
        }

        public async Task<ApiResult<PagedResult<NguyenVatLieuModel>>> GetNguyenVatLieuPaging(GetNguyenVatLieuPaging bundle)
        {
            var query = from s in _context.NguyenVatLieus
                        join g in _context.LoaiNguyenVatLieus on s.IdLoaiNVL equals g.Id
                        join d in _context.DonViTinhs on s.Id equals d.IdNguyenVatLieu
                        where d.CoBan == true

                        select new { s, g, d };

            if (bundle.IdLoaiNguyenVatLieu > 0)
            {
                query = query.Where(x => x.g.Id == bundle.IdLoaiNguyenVatLieu);
            }

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(c => c.s.Ten.Contains(bundle.TuKhoa) || c.s.MaSo.Contains(bundle.TuKhoa));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            query = query.OrderByDescending(c => c.s.Id);
            var data = await query.Skip((bundle.PageIndex - 1) * bundle.PageSize)
                .Take(bundle.PageSize)
                .Select(i => new NguyenVatLieuModel()
                {
                    Id = i.s.Id,
                    MaSo = i.s.MaSo,
                    Ten = i.s.Ten,
                    HinhAnh = i.s.HinhAnh,
                    SoLuong = i.s.SoLuong,
                    NhacNho = i.s.NhacNho,
                    TenDVTCoBan = i.d.Ten,
                    TenLoaiNVL = i.g.Ten,
                    MucTonCaoNhat = i.s.MucTonCaoNhat,
                    MucTonThapNhat = i.s.MucTonThapNhat
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<NguyenVatLieuModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<NguyenVatLieuModel>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var nguyenVatLieu = await _context.NguyenVatLieus.Include(x => x.DonViTinhs).FirstAsync(x => x.Id == id);
            if (nguyenVatLieu == null)
            {
                return new ApiErrorResult<bool>("Nguyên vật liệu không tồn tại");
            }

            if (nguyenVatLieu.HinhAnh != null)
            {
                await _storageService.DeleteFileAsync(nguyenVatLieu.HinhAnh);
            }

            _context.NguyenVatLieus.Remove(nguyenVatLieu);
            _context.DonViTinhs.RemoveRange(nguyenVatLieu.DonViTinhs);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            if (id != null)
            {
                var count = await _context.NguyenVatLieus
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(), SystemConstants.Collate_AS)
                    == ten.ToUpper().Trim() && c.Id != id);
                if (count)
                {
                    return new ApiErrorResult<bool>("Tên nguyên vật liệu đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                var count = await _context.NguyenVatLieus
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                    SystemConstants.Collate_AS) == ten.ToUpper().Trim());

                if (count)
                {
                    return new ApiErrorResult<bool>("Tên nguyên vật liệu đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<NguyenVatLieuShowAll>> GetShowAllNguyenVatLieu(int id)
        {
            var nguyenVatLieu = await _context.NguyenVatLieus.Include(x => x.LoaiNguyenVatLieu)
                .Include(x => x.DonViTinhs).FirstOrDefaultAsync(x => x.Id == id);
            if (nguyenVatLieu == null)
            {
                return new ApiErrorResult<NguyenVatLieuShowAll>("Nguyên vật liệu không tồn tại");
            }
            var result = new NguyenVatLieuShowAll()
            {
                Id = nguyenVatLieu.Id,
                MaSo = nguyenVatLieu.MaSo,
                Ten = nguyenVatLieu.Ten,
                HinhAnh = nguyenVatLieu.HinhAnh,
                IdLoaiNVL = nguyenVatLieu.IdLoaiNVL,
                TenLoaiNVL = nguyenVatLieu.LoaiNguyenVatLieu.Ten,
                MoTa = nguyenVatLieu.MoTa,
                SoLuong = nguyenVatLieu.SoLuong,
                NhacNho = nguyenVatLieu.NhacNho
            };

            result.TenDVTCoBan = nguyenVatLieu.DonViTinhs.Where(x => x.CoBan == true).Select(x => x.Ten).First();

            result.DonViTinhCDs = (List<DonViTinhCD>)nguyenVatLieu.DonViTinhs.Where(x => x.CoBan != true)
                                .Select(v => new DonViTinhCD()
                                {
                                    Id = v.Id,
                                    Ten = v.Ten,
                                    GiaTriChuyenDoi = v.GiaTriChuyenDoi,
                                    ChuyenDoi = (long)(result.SoLuong / v.GiaTriChuyenDoi)
                                }).ToList();

            return new ApiSuccessResult<NguyenVatLieuShowAll>(result);
        }

        public async Task<ApiResult<NguyenVatLieuModel>> Update(CapNhatNguyenVatLieu bundle)
        {
            var nguyenVatLieu = await _context.NguyenVatLieus.FindAsync(bundle.Id);
            if (nguyenVatLieu == null)
            {
                return new ApiErrorResult<NguyenVatLieuModel>("Nguyên vật liệu không tồn tại");
            }

            nguyenVatLieu.Ten = bundle.Ten;
            nguyenVatLieu.MoTa = bundle.MoTa;
            nguyenVatLieu.IdLoaiNVL = bundle.IdLoaiNguyenVatLieu;

            _context.NguyenVatLieus.Update(nguyenVatLieu);
            await _context.SaveChangesAsync();

            var result = new NguyenVatLieuModel()
            {
                Id = nguyenVatLieu.Id,
                HinhAnh = nguyenVatLieu.HinhAnh,
                Ten = nguyenVatLieu.Ten
            };
            var loaiNVL = await _context.LoaiNguyenVatLieus.FindAsync(nguyenVatLieu.IdLoaiNVL);
            result.TenLoaiNVL = loaiNVL.Ten;

            return new ApiSuccessResult<NguyenVatLieuModel>(result);
        }

        public async Task<ApiResult<NguyenVatLieuModel>> GetByUpdateNguyenVatLieu(int id)
        {
            var nguyenVatLieu = await _context.NguyenVatLieus.FindAsync(id);
            if (nguyenVatLieu == null)
            {
                return new ApiErrorResult<NguyenVatLieuModel>("Nguyên vật liệu không tồn tại");
            }
            var result = new NguyenVatLieuModel()
            {
                Id = nguyenVatLieu.Id,
                Ten = nguyenVatLieu.Ten,
                MoTa = nguyenVatLieu.MoTa,
                HinhAnh = nguyenVatLieu.HinhAnh,
                IdLoaiNVL = nguyenVatLieu.IdLoaiNVL
            };
            var loaiNVL = await _context.LoaiNguyenVatLieus.FindAsync(nguyenVatLieu.IdLoaiNVL);
            result.TenLoaiNVL = loaiNVL.Ten;

            return new ApiSuccessResult<NguyenVatLieuModel>(result);
        }

        public async Task<ApiResult<bool>> UpdateNhacNho(NhacNhoModel bundle)
        {
            var nhacNho = await _context.NguyenVatLieus.FindAsync(bundle.Id);
            if (nhacNho == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }
            nhacNho.NhacNho = bundle.NhacNho;
            if (bundle.MucTonCaoNhat != null)
            {
                nhacNho.MucTonCaoNhat = bundle.MucTonCaoNhat;
            }
            if (bundle.MucTonThapNhat != null)
            {
                nhacNho.MucTonThapNhat = (long)bundle.MucTonThapNhat;
            }
            _context.NguyenVatLieus.Update(nhacNho);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<NhacNhoModel>> GetNhacNho(int id)
        {
            var nhacNho = await _context.NguyenVatLieus.FindAsync(id);
            if (nhacNho == null)
            {
                return new ApiErrorResult<NhacNhoModel>("Id không tồn tại");
            }
            var result = new NhacNhoModel()
            {
                Id = nhacNho.Id,
                MucTonCaoNhat = nhacNho.MucTonCaoNhat,
                MucTonThapNhat = nhacNho.MucTonThapNhat,
                NhacNho = nhacNho.NhacNho
            };
            return new ApiSuccessResult<NhacNhoModel>(result);
        }
    }
}