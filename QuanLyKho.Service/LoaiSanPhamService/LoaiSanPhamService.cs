using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Date.EF;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.LoaiSanPhamService
{
    public class LoaiSanPhamService : ILoaiSanPhamService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly IMapper _mapper;

        public LoaiSanPhamService(QuanLyKhoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<LoaiSanPhamModel>> Create(TaoLoaiSanPham bundle)
        {
            var loaiSanPham = _mapper.Map<LoaiSanPham>(bundle);

            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var viTri = code.ViTri + stt;

            var str = code.Ten + viTri;

            var checkCode = await _context.LoaiSanPhams.AnyAsync(x => x.MaSo == str);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = viTri;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            loaiSanPham.MaSo = str;

            _context.LoaiSanPhams.Add(loaiSanPham);
            await _context.SaveChangesAsync();

            var result = new LoaiSanPhamModel()
            {
                Id = loaiSanPham.Id,
                MaSo = loaiSanPham.MaSo,
                Ten = loaiSanPham.Ten,
            };
            var nhomLoai = await _context.NhomSanPhams.FindAsync(bundle.IdNhomSanPham);
            result.NhomLoai = nhomLoai.Ten;

            return new ApiSuccessResult<LoaiSanPhamModel>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null)
            {
                return new ApiErrorResult<bool>("Loại nguyên vật liệu không tồn tại");
            }
            _context.LoaiSanPhams.Remove(loaiSanPham);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<LoaiSanPhamModel>> GetById(int id)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(id);
            if (loaiSanPham == null)
            {
                return new ApiErrorResult<LoaiSanPhamModel>("Nhóm loại  không tồn tại");
            }

            var nhomLoai = await _context.NhomSanPhams.FindAsync(loaiSanPham.IdNhomSanPham);

            var result = new LoaiSanPhamModel()
            {
                Id = loaiSanPham.Id,
                MaSo = loaiSanPham.MaSo,
                Ten = loaiSanPham.Ten,
                IdNhomSanPham = loaiSanPham.IdNhomSanPham,
                NhomLoai = nhomLoai.Ten
            };
            return new ApiSuccessResult<LoaiSanPhamModel>(result);
        }

        public async Task<ApiResult<PagedResult<LoaiSanPhamModel>>> GetLoaiSanPhamPaging(GetLoaiSanPham bundle)
        {
            var query = from s in _context.LoaiSanPhams
                        join g in _context.NhomSanPhams on s.IdNhomSanPham equals g.Id

                        select new { s, g };

            if (bundle.IdNhomLoai > 0)
            {
                query = query.Where(x => x.g.Id == bundle.IdNhomLoai);
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
                .Select(i => new LoaiSanPhamModel()
                {
                    Id = i.s.Id,
                    MaSo = i.s.MaSo,
                    Ten = i.s.Ten,
                    NhomLoai = i.g.Ten
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<LoaiSanPhamModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<LoaiSanPhamModel>>(pagedResult);
        }

        public async Task<List<GetTenLoaiSP>> GetTenLoaiSP()
        {
            var result = await _context.LoaiSanPhams.OrderBy(i => i.Ten)
                            .Select(i => new GetTenLoaiSP()
                            {
                                Id = i.Id,
                                Ten = i.Ten
                            }).ToListAsync();

            return result;
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            if (id != null)
            {
                var count = await _context.LoaiSanPhams
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(), SystemConstants.Collate_AS)
                    == ten.ToUpper().Trim() && c.Id != id);
                if (count)
                {
                    return new ApiErrorResult<bool>("Tên loại nguyên vật liệu đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                var count = await _context.LoaiSanPhams
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                    SystemConstants.Collate_AS) == ten.ToUpper().Trim());

                if (count)
                {
                    return new ApiErrorResult<bool>("Tên loại nguyên vật liệu đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> Update(int id, CapNhatLoaiSanPham bundle)
        {
            var loaiSanPham = await _context.LoaiSanPhams.FindAsync(bundle.Id);
            if (loaiSanPham == null)
            {
                return new ApiErrorResult<bool>("Loại nguyên vật liệu không tồn tại");
            }
            var list = _mapper.Map(bundle, loaiSanPham);
            _context.LoaiSanPhams.Update(list);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}