using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Date.EF;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NhaCungCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.NhaCungCapService
{
    public class NhaCungCapService : INhaCungCapService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly IMapper _mapper;

        public NhaCungCapService(QuanLyKhoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<NhaCungCapModel>> Create(TaoNhaCungCap bundle)
        {
            var supplier = _mapper.Map<NhaCungCap>(bundle);

            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var location = code.ViTri + stt;

            var str = code.Ten + location;

            var checkCode = await _context.NhaCungCaps.AnyAsync(x => x.MaSo == str);
            if (checkCode)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = location;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            supplier.MaSo = str;
            _context.NhaCungCaps.Add(supplier);
            await _context.SaveChangesAsync(); // số bản ghi nếu return

            var result = new NhaCungCapModel()
            {
                Id = supplier.Id,
                Ten = supplier.Ten,
                MaSo = supplier.MaSo,
                DiaChi = supplier.DiaChi,
                SoDienThoai = supplier.SoDienThoai,
                Email = supplier.Email,
                SoThue = supplier.SoThue
            };
            return new ApiSuccessResult<NhaCungCapModel>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var supplier = await _context.NhaCungCaps.FindAsync(id);
            if (supplier == null)
            {
                return new ApiErrorResult<bool>("Nhà cung cấp không tồn tại");
            }

            var reult = _context.NhaCungCaps.Remove(supplier);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<NhaCungCapModel>> GetById(int id)
        {
            var supplier = await _context.NhaCungCaps.FindAsync(id);
            if (supplier == null)
            {
                return new ApiErrorResult<NhaCungCapModel>("Người dùng không tồn tại");
            }
            var supplierVm = _mapper.Map<NhaCungCapModel>(supplier);

            return new ApiSuccessResult<NhaCungCapModel>(supplierVm);
        }

        public async Task<ApiResult<PagedResult<NhaCungCapModel>>> GetNhaCungCapPaging(GetNhaCungCapPagingRequest bundle)
        {
            IQueryable<NhaCungCap> query = _context.NhaCungCaps;

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(c => c.Ten.Contains(bundle.TuKhoa) || c.MaSo.Contains(bundle.TuKhoa));
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            query = query.OrderByDescending(c => c.Id);
            var data = await query.Skip((bundle.PageIndex - 1) * bundle.PageSize)
                .Take(bundle.PageSize)
                .Select(x => new NhaCungCapModel()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    SoThue = x.SoThue,
                    Ten = x.Ten,
                    SoDienThoai = x.SoDienThoai,
                    Email = x.Email,
                    DiaChi = x.DiaChi
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<NhaCungCapModel>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<NhaCungCapModel>>(pagedResult);
        }

        public async Task<ApiResult<bool>> iEmail(string email, int? id)
        {
            if (id != null)
            {
                if (await _context.NhaCungCaps.AnyAsync(x => x.Email == email && x.Id != id))
                {
                    return new ApiErrorResult<bool>("Email đã tồn tại nhà cung cấp");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                if (await _context.NhaCungCaps.AnyAsync(x => x.Email == email))
                {
                    return new ApiErrorResult<bool>("Email đã tồn tại nhà cung cấp");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> iMaThue(string thue, int? id)
        {
            if (id != null)
            {
                if (await _context.NhaCungCaps.AnyAsync(x => x.SoThue == thue && x.Id != id))
                {
                    return new ApiErrorResult<bool>("Mã số thuế đã tồn tại nhà cung cấp");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                if (await _context.NhaCungCaps.AnyAsync(x => x.SoThue == thue))
                {
                    return new ApiErrorResult<bool>("Mã số thuế đã tồn tại nhà cung cấp");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> Update(int id, CapNhatNhaCungCap bundle)
        {
            var user = await _context.NhaCungCaps.FindAsync(id);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Nhà cung cấp không tồn tại");
            }
            var list = _mapper.Map(bundle, user);

            _context.NhaCungCaps.Update(list);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }
    }
}