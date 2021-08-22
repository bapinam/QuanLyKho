using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Date.EF;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.QuanLyMaSo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.QuanLyMaSoService
{
    public class QuanLyMaSoService : IQuanLyMaSoService
    {
        private readonly QuanLyKhoDbContext _context;

        public QuanLyMaSoService(QuanLyKhoDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<bool>> Create(TaoMaSo bundle)
        {
            var check = await _context.QuanLyMaSos.AnyAsync(x => x.Ten == bundle.Ten);
            if (check)
            {
                return new ApiErrorResult<bool>("Mã số đã tồn tại");
            }
            var code = _context.QuanLyMaSos;

            var data = new QuanLyMaSo()
            {
                Ten = bundle.Ten,
                MaLoai = bundle.MaLoai
            };

            _context.QuanLyMaSos.Add(data);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var iCode = await _context.QuanLyMaSos.FindAsync(id);
            if (iCode == null)
            {
                return new ApiErrorResult<bool>();
            }

            _context.QuanLyMaSos.Remove(iCode);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }

        public async Task<List<LoaiMaSo>> GetAll(MaLoai bundle)
        {
            var code = _context.QuanLyMaSos
                .Where(x => x.MaLoai == bundle);

            var data = await code
                .OrderByDescending(x => x.DinhDau)
                .Select(x => new LoaiMaSo()
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    DinhDau = x.DinhDau
                }).ToListAsync();

            return new List<LoaiMaSo>(data);
        }

        public async Task<ApiResult<bool>> iTen(string ten, int? id)
        {
            if (id != null)
            {
                var count = await _context.QuanLyMaSos
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(), SystemConstants.Collate_AS)
                    == ten.ToUpper().Trim() && c.Id != id);
                if (count)
                {
                    return new ApiErrorResult<bool>("Tên mã đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                var count = await _context.QuanLyMaSos
                    .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                    SystemConstants.Collate_AS) == ten.ToUpper().Trim());

                if (count)
                {
                    return new ApiErrorResult<bool>("Tên mã đã tồn tại");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> Update(CapNhatMaSo bundle)
        {
            if (bundle.DinhDau)
            {
                var check = await _context.QuanLyMaSos
                    .AnyAsync(x => x.MaLoai == bundle.MaLoai && x.DinhDau == true);
                if (check)
                {
                    var bol = await _context.QuanLyMaSos
                    .Where(x => x.MaLoai == bundle.MaLoai && x.DinhDau == true).ToListAsync();

                    foreach (var i in bol)
                    {
                        i.DinhDau = false;
                        _context.QuanLyMaSos.Update(i);
                    }
                }
            }
            var code = await _context.QuanLyMaSos.FindAsync(bundle.Id);
            if (code == null)
            {
                return new ApiErrorResult<bool>();
            }

            code.Ten = bundle.Ten;
            code.DinhDau = bundle.DinhDau;

            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }
    }
}