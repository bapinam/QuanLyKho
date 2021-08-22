using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.LoaiNguyenVatLieuService
{
    public class LoaiNguyenVatLieuService : ILoaiNguyenVatLieuService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly IMapper _mapper;

        public LoaiNguyenVatLieuService(QuanLyKhoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<LoaiNguyenVatLieuModel>> Create(TaoLoaiNguyenVatLieu bundle)
        {
            var loaiNguyenVatLieu = _mapper.Map<LoaiNguyenVatLieu>(bundle);
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var location = code.ViTri + stt;

            var str = code.Ten + location;

            var checkCode = await _context.LoaiNguyenVatLieus.AnyAsync(x => x.MaSo == str);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = location;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            loaiNguyenVatLieu.MaSo = str;
            _context.LoaiNguyenVatLieus.Add(loaiNguyenVatLieu);
            await _context.SaveChangesAsync();

            var result = new LoaiNguyenVatLieuModel()
            {
                Id = loaiNguyenVatLieu.Id,
                MaSo = loaiNguyenVatLieu.MaSo,
                Ten = loaiNguyenVatLieu.Ten,
                NhomLoai = loaiNguyenVatLieu.NhomLoaiNVL == NhomLoaiNVL.NguyenLieu ? "Nguyên Liệu" :
                              loaiNguyenVatLieu.NhomLoaiNVL == NhomLoaiNVL.NhienLieu ? "Nhiên Liệu" : "Vật Liệu"
            };
            return new ApiSuccessResult<LoaiNguyenVatLieuModel>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var loaiNguyenVatLieu = await _context.LoaiNguyenVatLieus.FindAsync(id);
            if (loaiNguyenVatLieu == null)
            {
                return new ApiErrorResult<bool>("Loại nguyên vật liệu không tồn tại");
            }
            _context.LoaiNguyenVatLieus.Remove(loaiNguyenVatLieu);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<LoaiNguyenVatLieuModel>> GetById(int id)
        {
            var loaiNguyenVatLieu = await _context.LoaiNguyenVatLieus.FindAsync(id);
            if (loaiNguyenVatLieu == null)
            {
                return new ApiErrorResult<LoaiNguyenVatLieuModel>("Loại nguyên vật liệu không tồn tại");
            }
            var result = new LoaiNguyenVatLieuModel()
            {
                Id = loaiNguyenVatLieu.Id,
                MaSo = loaiNguyenVatLieu.MaSo,
                Ten = loaiNguyenVatLieu.Ten,
                NhomLoai = loaiNguyenVatLieu.NhomLoaiNVL == NhomLoaiNVL.NguyenLieu ? "Nguyên Liệu" :
                              loaiNguyenVatLieu.NhomLoaiNVL == NhomLoaiNVL.NhienLieu ? "Nhiên Liệu" : "Vật Liệu"
            };

            return new ApiSuccessResult<LoaiNguyenVatLieuModel>(result);
        }

        public async Task<ApiResult<PagedResult<LoaiNguyenVatLieuModel>>> GetLoaiNguyenVatLieuPaging(GetLoaiNguyenVatLieu bundle)
        {
            IQueryable<LoaiNguyenVatLieu> query = _context.LoaiNguyenVatLieus;

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(c => c.Ten.Contains(bundle.TuKhoa) || c.MaSo.Contains(bundle.TuKhoa));
            }

            if (bundle.NhomLoai != null)
            {
                query = query.Where(c => c.NhomLoaiNVL == bundle.NhomLoai);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            query = query.OrderByDescending(c => c.Id);
            var data = await query.Skip((bundle.PageIndex - 1) * bundle.PageSize)
                .Take(bundle.PageSize)
                .Select(i => new LoaiNguyenVatLieuModel()
                {
                    Id = i.Id,
                    MaSo = i.MaSo,
                    Ten = i.Ten,
                    NhomLoai = i.NhomLoaiNVL == NhomLoaiNVL.NguyenLieu ? "Nguyên Liệu" :
                              i.NhomLoaiNVL == NhomLoaiNVL.NhienLieu ? "Nhiên Liệu" : "Vật Liệu"
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<LoaiNguyenVatLieuModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<LoaiNguyenVatLieuModel>>(pagedResult);
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            if (id != null)
            {
                var count = await _context.LoaiNguyenVatLieus
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
                var count = await _context.LoaiNguyenVatLieus
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

        public async Task<ApiResult<bool>> Update(int id, CapNhatLoaiNguyenVatLieu bundle)
        {
            var loaiNguyenVatLieu = await _context.LoaiNguyenVatLieus.FindAsync(bundle.Id);
            if (loaiNguyenVatLieu == null)
            {
                return new ApiErrorResult<bool>("Loại nguyên vật liệu không tồn tại");
            }
            var list = _mapper.Map(bundle, loaiNguyenVatLieu);
            _context.LoaiNguyenVatLieus.Update(list);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<List<GetTenLoaiNVL>> GetTenLoaiNVL()
        {
            var result = await _context.LoaiNguyenVatLieus.OrderBy(i => i.Ten)
                .Select(i => new GetTenLoaiNVL()
                {
                    Id = i.Id,
                    Ten = i.Ten
                }).ToListAsync();

            return result;
        }
    }
}