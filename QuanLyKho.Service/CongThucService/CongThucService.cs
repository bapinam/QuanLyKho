using QuanLyKho.Date.EF;
using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyKho.ViewModels.CongThuc;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Data.Entities;
using QuanLyKho.Utilities.Constants;

namespace QuanLyKho.Service.CongThucService
{
    public class CongThucService : ICongThucService
    {
        private readonly QuanLyKhoDbContext _context;

        public CongThucService(QuanLyKhoDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<GetTaoCongThuc>> Create(TaoCongThuc bundle)
        {
            var count = _context.CongThucs.Where(x => x.IdSanPham == bundle.IdSanPham).Count();
            if (count < 1)
            {
                bundle.DinhDau = true;
            }

            if (bundle.DinhDau)
            {
                var check = await _context.CongThucs
               .AnyAsync(x => x.IdSanPham == bundle.IdSanPham && x.DinhDau == true);

                if (check)
                {
                    var reciper = await _context.CongThucs
                        .FirstAsync(x => x.IdSanPham == bundle.IdSanPham && x.DinhDau == true);

                    reciper.DinhDau = false;
                    _context.CongThucs.Update(reciper);
                }
            }

            var recipeDetail = new List<ChiTietCongThuc>();
            if (bundle.IdNguyenVatLieu != null)
            {
                int i = 0;
                foreach (int item in bundle.IdNguyenVatLieu)
                {
                    recipeDetail.Add(new ChiTietCongThuc()
                    {
                        IdNguyenVatLieu = item,
                        SoLuong = bundle.SoLuong[i],
                        DonViTinh = bundle.DonViTinh[i]
                    });
                    i++;
                }
            }

            var recipe = new CongThuc()
            {
                Ten = bundle.Ten,
                GhiChu = bundle.GhiChu,
                DinhDau = bundle.DinhDau,
                IdSanPham = bundle.IdSanPham,
                ChiTietCongThucs = recipeDetail
            };

            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var viTri = code.ViTri + stt;

            var str = code.Ten + viTri;

            var checkMaSo = await _context.CongThucs.AnyAsync(x => x.MaSo == str);
            if (checkMaSo)
            {
                stt++;
                goto ViTri;
            }

            code.ViTri = viTri;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            recipe.MaSo = str;

            _context.CongThucs.Add(recipe);
            await _context.SaveChangesAsync();

            var result = new GetTaoCongThuc()
            {
                IdSanPham = recipe.IdSanPham,
                TenCongThuc = recipe.Ten,
                DinhDau = recipe.DinhDau
            };
            return new ApiSuccessResult<GetTaoCongThuc>(result);
        }

        public async Task<ApiResult<string>> Delete(int id, int idSanPham)
        {
            string result;
            var check = await _context.CongThucs.AnyAsync(x => x.Id == id && x.DinhDau == true);
            if (check)
            {
                var proudct = _context.CongThucs
                    .Where(x => x.IdSanPham == idSanPham).Count();

                if (proudct > 1)
                {
                    var list = await _context.CongThucs
                   .FirstAsync(x => x.IdSanPham == idSanPham && x.DinhDau == false);

                    list.DinhDau = true;
                    _context.CongThucs.Update(list);
                    result = list.Ten;
                }
                else
                {
                    result = "Chưa có công thức";
                }
            }
            else
            {
                result = "";
            }

            var reciper = await _context.CongThucs.Include(x => x.ChiTietCongThucs)
                .FirstAsync(x => x.Id == id);

            _context.CongThucs.Remove(reciper);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<string>(result);
        }

        public async Task<ApiResult<bool>> DeleteNguyenVatLieu(long id)
        {
            var delete = await _context.ChiTietCongThucs.FindAsync(id);
            if (delete == null)
            {
                return new ApiErrorResult<bool>("Dữ liệu không tồn tại");
            }

            _context.ChiTietCongThucs.Remove(delete);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<PagedResult<SanPhamCongThuc>>> GetCongThucSPPaging(GetCongThucSPPaging bundle)
        {
            var query = from s in _context.SanPhams
                        join g in _context.LoaiSanPhams on s.IdLoaiSanPham equals g.Id
                        join d in _context.CongThucs on s.Id equals d.IdSanPham into r
                        from d in r.DefaultIfEmpty()
                        where d.DinhDau != false
                        select new
                        {
                            s,
                            d = (d == null) ? "Chưa có công thức" : d.Ten,
                            g
                        };

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
                .Select(i => new SanPhamCongThuc()
                {
                    Id = i.s.Id,
                    MaSo = i.s.MaSo,
                    Ten = i.s.Ten,
                    HinhAnh = i.s.HinhAnh,
                    TenLoaiSanPham = i.g.Ten,
                    TenCongThuc = i.d
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<SanPhamCongThuc>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };

            return new ApiSuccessResult<PagedResult<SanPhamCongThuc>>(pagedResult);
        }

        public async Task<List<GetListCongThuc>> GetListCongThuc(int id)
        {
            var recipes = await _context.CongThucs.Include(rd => rd.ChiTietCongThucs)
                            .ThenInclude(m => m.NguyenVatLieu).Where(x => x.IdSanPham == id).ToListAsync();

            var result = recipes
                .OrderByDescending(x => x.DinhDau)
                .Select(i => new GetListCongThuc()
                {
                    Id = i.Id,
                    MaSo = i.MaSo,
                    Ten = i.Ten,
                    DinhDau = i.DinhDau,
                    GhiChu = i.GhiChu,
                    IngredientCongThucs = i.ChiTietCongThucs.Select(x => new IngredientCongThuc()
                    {
                        Id = x.Id,
                        SoLuong = x.SoLuong,
                        DonViTinh = x.DonViTinh,
                        Ten = x.NguyenVatLieu.Ten
                    }).ToList()
                }).ToList();

            return new List<GetListCongThuc>(result);
        }

        public async Task<List<GetListDVTSanPham>> GetListDonViTinhs(int id)
        {
            var packs = _context.DonViTinhs.Where(x => x.IdNguyenVatLieu == id);

            var result = await packs.Select(x => new GetListDVTSanPham()
            {
                Id = x.Id,
                Ten = x.Ten,
                CoBan = x.CoBan
            }
            ).ToListAsync();

            return new List<GetListDVTSanPham>(result);
        }

        public async Task<ApiResult<GetListIdCongThuc>> GetListIdCongThuc(int id)
        {
            var check = await _context.CongThucs.FindAsync(id);
            if (check == null)
            {
                return new ApiErrorResult<GetListIdCongThuc>();
            }
            var recipes = _context.CongThucs.Include(x => x.ChiTietCongThucs)
                .ThenInclude(x => x.NguyenVatLieu)
                .Where(x => x.Id == id);

            var result = await recipes.Select(x => new GetListIdCongThuc()
            {
                Id = x.Id,
                MaSo = x.MaSo,
                Ten = x.Ten,
                DinhDau = x.DinhDau,
                GhiChu = x.GhiChu,
                IngredientCongThucs = x.ChiTietCongThucs.Select(i => new IngredientCongThuc()
                {
                    Id = i.Id,
                    Ten = i.NguyenVatLieu.Ten,
                    SoLuong = i.SoLuong,
                    DonViTinh = i.DonViTinh
                }).ToList()
            }).FirstAsync();

            return new ApiSuccessResult<GetListIdCongThuc>(result);
        }

        public async Task<List<GetListNguyenVatLieu>> GetListNguyenVatLieu(ListBundleNguyenVatLieu bundle)
        {
            var materialsType = _context.NguyenVatLieus.Include(x => x.LoaiNguyenVatLieu);

            List<GetListNguyenVatLieu> result;
            if (bundle.NhomLoaiNVL != null)
            {
                result = await materialsType
                    .Where(
                        x => x.LoaiNguyenVatLieu.NhomLoaiNVL == bundle.NhomLoaiNVL
                        && x.IdLoaiNVL == bundle.LoaiNguyenVatLieu
                        && (x.Ten.Contains(bundle.TuKhoaNVL)
                        || x.MaSo.Contains(bundle.TuKhoaNVL)))
                    .Select(x => new GetListNguyenVatLieu()
                    {
                        Id = x.Id,
                        MaSo = x.MaSo,
                        Ten = x.Ten,
                        HinhAnh = x.HinhAnh,
                        TenLoai = x.LoaiNguyenVatLieu.Ten
                    }).ToListAsync();
            }
            else
            {
                result = await materialsType
                     .Where(x => x.IdLoaiNVL == bundle.LoaiNguyenVatLieu
                     && (x.Ten.Contains(bundle.TuKhoaNVL)
                        || x.MaSo.Contains(bundle.TuKhoaNVL)))
                    .Select(x => new GetListNguyenVatLieu()
                    {
                        Id = x.Id,
                        MaSo = x.MaSo,
                        Ten = x.Ten,
                        HinhAnh = x.HinhAnh,
                        TenLoai = x.LoaiNguyenVatLieu.Ten
                    }).ToListAsync();
            }

            return new List<GetListNguyenVatLieu>(result);
        }

        public async Task<List<GetLoaiNguyenVatLieu>> GetLoaiNguyenVatLieu(NhomLoaiNVL? group)
        {
            var materialsType = _context.LoaiNguyenVatLieus;

            List<GetLoaiNguyenVatLieu> result;
            if (group != null)
            {
                result = await materialsType.Where(x => x.NhomLoaiNVL == group)
                    .Select(x => new GetLoaiNguyenVatLieu()
                    {
                        Id = x.Id,
                        MaSo = x.MaSo,
                        Ten = x.Ten
                    }).ToListAsync();
            }
            else
            {
                result = await materialsType
                    .Select(x => new GetLoaiNguyenVatLieu()
                    {
                        Id = x.Id,
                        MaSo = x.MaSo,
                        Ten = x.Ten
                    }).ToListAsync();
            }

            return new List<GetLoaiNguyenVatLieu>(result);
        }

        public async Task<ApiResult<bool>> iNguyenVatLieu(int idCongThuc, int idNguyenVatLieu)
        {
            var check = await _context.ChiTietCongThucs
                .AnyAsync(x => x.IdCongThuc == idCongThuc && x.IdNguyenVatLieu == idNguyenVatLieu);
            if (check)
            {
                return new ApiErrorResult<bool>("Dữ liệu đã tồn tại");
            }

            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<bool>> iTen(string ten, int idSP, int? id)
        {
            bool count;
            if (id != null)
            {
                count = await _context.CongThucs
                    .Where(x => x.IdSanPham == idSP && x.Id != id)
                   .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                   SystemConstants.Collate_AS) == ten.ToUpper().Trim());
            }
            else
            {
                count = await _context.CongThucs
                   .Where(x => x.IdSanPham == idSP)
                  .AnyAsync(c => EF.Functions.Collate(c.Ten.ToUpper().Trim(),
                  SystemConstants.Collate_AS) == ten.ToUpper().Trim());
            }

            if (count)
            {
                return new ApiErrorResult<bool>("Tên đơn vị tính đã tồn tại");
            }
            else
            {
                return new ApiSuccessResult<bool>();
            }
        }

        public async Task<ApiResult<GetTaoCongThuc>> Update(CapNhatCongThuc bundle)
        {
            var data = await _context.CongThucs.FindAsync(bundle.IdCongThuc);

            data.IdSanPham = bundle.IdSanPham;
            data.Ten = bundle.Ten;
            data.GhiChu = bundle.GhiChu;

            _context.CongThucs.Update(data);

            // thêm nguyên liệu
            if (bundle.IdNguyenVatLieu != null)
            {
                int i = 0;
                foreach (int item in bundle.IdNguyenVatLieu)
                {
                    var recipeDetail = new ChiTietCongThuc()
                    {
                        IdNguyenVatLieu = item,
                        SoLuong = bundle.SoLuong[i],
                        DonViTinh = bundle.DonViTinh[i],
                        IdCongThuc = data.Id
                    };

                    _context.ChiTietCongThucs.Add(recipeDetail);

                    i++;
                }
            }

            await _context.SaveChangesAsync();

            var result = new GetTaoCongThuc()
            {
                IdSanPham = data.IdSanPham,
                TenCongThuc = data.Ten,
                DinhDau = data.DinhDau
            };
            return new ApiSuccessResult<GetTaoCongThuc>(result);
        }

        public async Task<ApiResult<string>> UpdateDinhDauCongThuc(CapNhatDinhDauCongThuc bundle)
        {
            var reciper = await _context.CongThucs
                      .FirstAsync(x => x.IdSanPham == bundle.IdSanPham && x.DinhDau == true);

            reciper.DinhDau = false;
            _context.CongThucs.Update(reciper);

            var prioritize = await _context.CongThucs.FirstAsync(x => x.Id == bundle.Id);
            prioritize.DinhDau = true;
            _context.CongThucs.Update(prioritize);

            await _context.SaveChangesAsync();

            return new ApiSuccessResult<string>(prioritize.Ten);
        }
    }
}