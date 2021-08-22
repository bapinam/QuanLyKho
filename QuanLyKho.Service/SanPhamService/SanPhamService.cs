using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.Common;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.SanPhamService
{
    public class SanPhamService : ISanPhamService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly IFileStorageService _storageService;
        private const string PRODUCT_CONTENT_FOLDER_NAME = SystemConstants.ImageFolder;

        public SanPhamService(QuanLyKhoDbContext context, IFileStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<ApiResult<string>> CapNhatHinhAnh(int id, CapNhatHinhAnhSP bundle)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return new ApiErrorResult<string>("Sản phẩm không tồn tại");
            }
            // xóa ảnh củ
            if (sanPham.HinhAnh != null)
            {
                await _storageService.DeleteFileAsync(sanPham.HinhAnh);
            }

            // lưu lại ảnh mới
            sanPham.HinhAnh = await this.SaveFile(bundle.HinhAnh);
            _context.SanPhams.Update(sanPham);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<string>(sanPham.HinhAnh);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + PRODUCT_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<ApiResult<SanPhamModel>> Create(TaoSanPham bundle)
        {
            // thêm đơn vị tính
            var pack = new List<DonViTinh>();
            pack.Add(new DonViTinh()
            {
                Ten = bundle.TenDVTCoBan,
                GiaTriChuyenDoi = 1,
                LoaiDongGoi = LoaiDongGoi.SanPham,
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
                        LoaiDongGoi = LoaiDongGoi.SanPham,
                        CoBan = false
                    });
                    i++;
                }
            }

            // thêm sản phẩm
            var sanPham = new SanPham()
            {
                MaSo = bundle.MaSo,
                Ten = bundle.Ten,
                MoTa = bundle.MoTa,
                IdLoaiSanPham = bundle.IdLoaiSanPham,
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

            var checkCode = await _context.SanPhams.AnyAsync(x => x.MaSo == str);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = viTri;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            sanPham.MaSo = str;
            _context.SanPhams.Add(sanPham);
            await _context.SaveChangesAsync();

            // trả về kết quả sau khi lưu thành công
            var ketQua = new SanPhamModel()
            {
                Id = sanPham.Id,
                MaSo = sanPham.MaSo,
                Ten = sanPham.Ten,
                SoLuong = sanPham.SoLuong,
                NhacNho = sanPham.NhacNho,
                TenDVTCoBan = bundle.TenDVTCoBan,
                MucTonCaoNhat = sanPham.MucTonCaoNhat,
                MucTonThapNhat = sanPham.MucTonThapNhat
            };
            var loaiSP = await _context.LoaiSanPhams.FindAsync(sanPham.IdLoaiSanPham);
            ketQua.TenLoaiSP = loaiSP.Ten;

            return new ApiSuccessResult<SanPhamModel>(ketQua);
        }

        public async Task<ApiResult<PagedResult<SanPhamModel>>> GetSanPhamPaging(GetSanPhamPaging bundle)
        {
            var query = from s in _context.SanPhams
                        join g in _context.LoaiSanPhams on s.IdLoaiSanPham equals g.Id
                        join d in _context.DonViTinhs on s.Id equals d.IdSanPham
                        where d.CoBan == true

                        select new { s, g, d };

            if (bundle.IdLoaiSanPham > 0)
            {
                query = query.Where(x => x.g.Id == bundle.IdLoaiSanPham);
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
                .Select(i => new SanPhamModel()
                {
                    Id = i.s.Id,
                    MaSo = i.s.MaSo,
                    Ten = i.s.Ten,
                    HinhAnh = i.s.HinhAnh,
                    SoLuong = i.s.SoLuong,
                    NhacNho = i.s.NhacNho,
                    TenDVTCoBan = i.d.Ten,
                    TenLoaiSP = i.g.Ten,
                    MucTonCaoNhat = i.s.MucTonCaoNhat,
                    MucTonThapNhat = i.s.MucTonThapNhat
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<SanPhamModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<SanPhamModel>>(pagedResult);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var sanPham = await _context.SanPhams.Include(x => x.DonViTinhs).FirstAsync(x => x.Id == id);
            if (sanPham == null)
            {
                return new ApiErrorResult<bool>("Sản phẩm không tồn tại");
            }

            if (sanPham.HinhAnh != null)
            {
                await _storageService.DeleteFileAsync(sanPham.HinhAnh);
            }

            _context.SanPhams.Remove(sanPham);
            _context.DonViTinhs.RemoveRange(sanPham.DonViTinhs);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            if (id != null)
            {
                var count = await _context.SanPhams
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(), SystemConstants.Collate_AS)
                    == ten.ToUpper().Trim() && c.Id != id);
                if (count)
                {
                    return new ApiErrorResult<bool>("Tên sản phẩm đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                var count = await _context.SanPhams
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                    SystemConstants.Collate_AS) == ten.ToUpper().Trim());

                if (count)
                {
                    return new ApiErrorResult<bool>("Tên sản phẩm đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<SanPhamShowAll>> GetShowAllSanPham(int id)
        {
            var sanPham = await _context.SanPhams.Include(x => x.LoaiSanPham)
                .Include(x => x.DonViTinhs).FirstOrDefaultAsync(x => x.Id == id);
            if (sanPham == null)
            {
                return new ApiErrorResult<SanPhamShowAll>("Sản phẩm không tồn tại");
            }
            var result = new SanPhamShowAll()
            {
                Id = sanPham.Id,
                MaSo = sanPham.MaSo,
                Ten = sanPham.Ten,
                HinhAnh = sanPham.HinhAnh,
                IdLoaiSP = sanPham.IdLoaiSanPham,
                TenLoaiSP = sanPham.LoaiSanPham.Ten,
                MoTa = sanPham.MoTa,
                SoLuong = sanPham.SoLuong,
                NhacNho = sanPham.NhacNho
            };

            result.TenDVTCoBan = sanPham.DonViTinhs.Where(x => x.CoBan == true).Select(x => x.Ten).First();

            result.DonViTinhCDs = (List<DonViTinhCD>)sanPham.DonViTinhs.Where(x => x.CoBan != true)
                                .Select(v => new DonViTinhCD()
                                {
                                    Id = v.Id,
                                    Ten = v.Ten,
                                    GiaTriChuyenDoi = v.GiaTriChuyenDoi,
                                    ChuyenDoi = (long)(result.SoLuong / v.GiaTriChuyenDoi)
                                }).ToList();

            return new ApiSuccessResult<SanPhamShowAll>(result);
        }

        public async Task<ApiResult<SanPhamModel>> Update(CapNhatSanPham bundle)
        {
            var sanPham = await _context.SanPhams.FindAsync(bundle.Id);
            if (sanPham == null)
            {
                return new ApiErrorResult<SanPhamModel>("Sản phẩm không tồn tại");
            }

            sanPham.Ten = bundle.Ten;
            sanPham.MoTa = bundle.MoTa;
            sanPham.IdLoaiSanPham = bundle.IdLoaiSanPham;

            _context.SanPhams.Update(sanPham);
            await _context.SaveChangesAsync();

            var result = new SanPhamModel()
            {
                Id = sanPham.Id,
                HinhAnh = sanPham.HinhAnh,
                Ten = sanPham.Ten
            };
            var loaiSP = await _context.LoaiSanPhams.FindAsync(sanPham.IdLoaiSanPham);
            result.TenLoaiSP = loaiSP.Ten;

            return new ApiSuccessResult<SanPhamModel>(result);
        }

        public async Task<ApiResult<SanPhamModel>> GetByUpdateSanPham(int id)
        {
            var sanPham = await _context.SanPhams.FindAsync(id);
            if (sanPham == null)
            {
                return new ApiErrorResult<SanPhamModel>("Sản phẩm không tồn tại");
            }
            var result = new SanPhamModel()
            {
                Id = sanPham.Id,
                Ten = sanPham.Ten,
                MoTa = sanPham.MoTa,
                HinhAnh = sanPham.HinhAnh,
                IdLoaiSP = sanPham.IdLoaiSanPham
            };
            var loaiSP = await _context.LoaiSanPhams.FindAsync(sanPham.IdLoaiSanPham);
            result.TenLoaiSP = loaiSP.Ten;

            return new ApiSuccessResult<SanPhamModel>(result);
        }

        public async Task<ApiResult<bool>> UpdateNhacNho(NhacNhoSPModel bundle)
        {
            var nhacNho = await _context.SanPhams.FindAsync(bundle.Id);
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
            _context.SanPhams.Update(nhacNho);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<NhacNhoSPModel>> GetNhacNho(int id)
        {
            var nhacNho = await _context.SanPhams.FindAsync(id);
            if (nhacNho == null)
            {
                return new ApiErrorResult<NhacNhoSPModel>("Id không tồn tại");
            }
            var result = new NhacNhoSPModel()
            {
                Id = nhacNho.Id,
                MucTonCaoNhat = nhacNho.MucTonCaoNhat,
                MucTonThapNhat = nhacNho.MucTonThapNhat,
                NhacNho = nhacNho.NhacNho
            };
            return new ApiSuccessResult<NhacNhoSPModel>(result);
        }
    }
}