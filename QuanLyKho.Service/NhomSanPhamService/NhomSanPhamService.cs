using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Date.EF;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NhomSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.NhomSanPhamService
{
    public class NhomSanPhamService : INhomSanPhamService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly IMapper _mapper;

        public NhomSanPhamService(QuanLyKhoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<NhomSanPhamModel>> Create(TaoNhomSanPham bundle)
        {
            var nhomSanPham = _mapper.Map<NhomSanPham>(bundle);
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            Location:
            var location = code.ViTri + stt;

            var str = code.Ten + location;

            var checkCode = await _context.NhomSanPhams.AnyAsync(x => x.MaSo == str);
            if (checkCode)
            {
                stt++;
                goto Location;
            }

            code.ViTri = location;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            nhomSanPham.MaSo = str;
            _context.NhomSanPhams.Add(nhomSanPham);
            await _context.SaveChangesAsync();

            var result = new NhomSanPhamModel()
            {
                Id = nhomSanPham.Id,
                MaSo = nhomSanPham.MaSo,
                Ten = nhomSanPham.Ten
            };
            return new ApiSuccessResult<NhomSanPhamModel>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var nhomSanPham = await _context.NhomSanPhams.FindAsync(id);
            if (nhomSanPham == null)
            {
                return new ApiErrorResult<bool>("Nhóm loại không tồn tại");
            }
            _context.NhomSanPhams.Remove(nhomSanPham);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<NhomSanPhamModel>> GetById(int id)
        {
            var nhomSanPham = await _context.NhomSanPhams.FindAsync(id);
            if (nhomSanPham == null)
            {
                return new ApiErrorResult<NhomSanPhamModel>("Nhóm loại  không tồn tại");
            }

            var result = _mapper.Map<NhomSanPhamModel>(nhomSanPham);

            return new ApiSuccessResult<NhomSanPhamModel>(result);
        }

        public async Task<ApiResult<PagedResult<NhomSanPhamModel>>> GetNhomSanPhamPaging(GetNhomSanPham bundle)
        {
            IQueryable<NhomSanPham> query = _context.NhomSanPhams;

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(c => c.Ten.Contains(bundle.TuKhoa) || c.MaSo.Contains(bundle.TuKhoa));
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            query = query.OrderByDescending(c => c.Id);
            var data = await query.Skip((bundle.PageIndex - 1) * bundle.PageSize)
                .Take(bundle.PageSize)
                .Select(i => new NhomSanPhamModel()
                {
                    Id = i.Id,
                    MaSo = i.MaSo,
                    Ten = i.Ten
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<NhomSanPhamModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<NhomSanPhamModel>>(pagedResult);
        }

        public async Task<List<GetTenNhomSP>> GetTenNhomSP()
        {
            var result = await _context.NhomSanPhams.OrderBy(i => i.Ten)
                            .Select(i => new GetTenNhomSP()
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
                var count = await _context.NhomSanPhams
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(), SystemConstants.Collate_AS)
                    == ten.ToUpper().Trim() && c.Id != id);
                if (count)
                {
                    return new ApiErrorResult<bool>("Tên nhóm loại đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                var count = await _context.NhomSanPhams
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                    SystemConstants.Collate_AS) == ten.ToUpper().Trim());

                if (count)
                {
                    return new ApiErrorResult<bool>("Tên nhóm loại đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> Update(int id, CapNhatNhomSanPham bundle)
        {
            var nhomSanPham = await _context.NhomSanPhams.FindAsync(bundle.Id);
            if (nhomSanPham == null)
            {
                return new ApiErrorResult<bool>("Nhóm loại không tồn tại");
            }
            var list = _mapper.Map(bundle, nhomSanPham);
            _context.NhomSanPhams.Update(list);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }
    }
}