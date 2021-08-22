using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuanLyKho.Data.Entities;
using QuanLyKho.Date.EF;
using QuanLyKho.ViewModels.ChiMuc;
using QuanLyKho.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Service.ChiMucService
{
    public class ChiMucService : IChiMucService
    {
        private readonly QuanLyKhoDbContext _context;

        public ChiMucService(QuanLyKhoDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<string>> GetName(string ten)
        {
            var stt = 0;
            KiemTra:
            stt++;
            bool chiMuc = await _context.ChiMucs.AnyAsync(x => x.Ten == ten);
            if (chiMuc)
            {
                ten = ten + " - Mục 0" + stt;
                goto KiemTra;
            }

            return new ApiSuccessResult<string>(ten);
        }

        public async Task<ApiResult<ModelChiMucGetOne>> Create(TaoChiMuc bundle)
        {
            var str = bundle.Ngay.Split(" - ");
            DateTime ngayBD = Convert.ToDateTime(str[0]);
            DateTime ngayKT = Convert.ToDateTime(str[1]);

            var iNgayBD = await this.iNgayTonTai(ngayBD, null);
            if (iNgayBD.IsSuccessed)
            {
                return new ApiErrorResult<ModelChiMucGetOne>("Ngày bắt đầu  đã tồn tại");
            }
            var iNgayKT = await this.iNgayTonTai(ngayKT, null);
            if (iNgayKT.IsSuccessed)
            {
                return new ApiErrorResult<ModelChiMucGetOne>("Ngày kết thúc đã tồn tại");
            }

            string ngayMD = "";
            if (bundle.NgayNghiMacDinh != null)
            {
                foreach (var item in bundle.NgayNghiMacDinh)
                {
                    ngayMD = ngayMD + item + ";";
                }
            }

            var soNgayLamViec = this.TinhToanSoNgayLamViec(ngayBD, ngayKT, ngayMD, bundle.NgayNghi);

            var index = new ChiMuc()
            {
                Ten = bundle.Ten,
                NgayBatDau = ngayBD,
                NgayKetThuc = ngayKT,
                SoNgayLamViec = soNgayLamViec,
                NgayNghiMacDinh = ngayMD
            };

            if (bundle.NgayNghi != null)
            {
                List<NgayNghiTuChon> ngayNghiTuChons = new List<NgayNghiTuChon>();
                foreach (var item in bundle.NgayNghi)
                {
                    ngayNghiTuChons.Add(
                        new NgayNghiTuChon()
                        {
                            NgayNghi = item
                        });
                }

                index.NgayNghiTuChons = ngayNghiTuChons;
            }

            _context.ChiMucs.Add(index);
            await _context.SaveChangesAsync();

            var result = new ModelChiMucGetOne()
            {
                Id = index.Id,
                Ten = index.Ten,
                NgayKetThuc = index.NgayKetThuc,
                NgayBatDau = index.NgayBatDau,
                NgayBatDauFormat = index.NgayBatDau.ToString("dd/MM/yyyy"),
                NgayKetThucFormat = index.NgayKetThuc.ToString("dd/MM/yyyy"),
                SoNgayLamViec = index.SoNgayLamViec
            };
            return new ApiSuccessResult<ModelChiMucGetOne>(result);
        }

        public async Task<ApiResult<bool>> Delete(int id)
        {
            var index = await _context.ChiMucs.Include(x => x.NgayNghiTuChons).FirstOrDefaultAsync(x => x.Id == id);
            if (index == null)
            {
                return new ApiErrorResult<bool>("Id không tồn tại");
            }

            _context.NgayNghiTuChons.RemoveRange(index.NgayNghiTuChons);
            _context.ChiMucs.Remove(index);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        public async Task<ApiResult<UpdateChiMucModel>> DeleteNgayNghi(int id)
        {
            var ngay = await _context.NgayNghiTuChons.FindAsync(id);
            if (ngay == null)
            {
                return new ApiErrorResult<UpdateChiMucModel>("Id không tồn tại");
            }
            var idChiMuc = ngay.IdChiMuc;
            _context.NgayNghiTuChons.Remove(ngay);
            await _context.SaveChangesAsync();

            var chiMuc = await _context.ChiMucs.Include(x => x.NgayNghiTuChons).FirstOrDefaultAsync(x => x.Id == idChiMuc);
            // cập nhật lại ngày
            int soNgayLamViec = 0;
            if (chiMuc.NgayNghiTuChons != null)
            {
                DateTime[] ngayNghiTuChon = chiMuc.NgayNghiTuChons.Select(x => x.NgayNghi).ToArray();
                soNgayLamViec = this.TinhToanSoNgayLamViec(chiMuc.NgayBatDau, chiMuc.NgayKetThuc, chiMuc.NgayNghiMacDinh, ngayNghiTuChon);
            }
            else
            {
                soNgayLamViec = this.TinhToanSoNgayLamViec(chiMuc.NgayBatDau, chiMuc.NgayKetThuc, chiMuc.NgayNghiMacDinh);
            }

            chiMuc.SoNgayLamViec = soNgayLamViec;
            _context.ChiMucs.Update(chiMuc);
            await _context.SaveChangesAsync();
            var result = new UpdateChiMucModel()
            {
                Id = chiMuc.Id,
                SoNgayLamViec = chiMuc.SoNgayLamViec
            };
            return new ApiSuccessResult<UpdateChiMucModel>(result);
        }

        public async Task<ApiResult<PagedResult<ModelChimuc>>> GetAll(GetAllChiMuc bundle)
        {
            var index = _context.ChiMucs;
            IQueryable<ChiMuc> query = index.OrderByDescending(x => x.NgayKetThuc);

            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = index.Where(x => x.Ten.Contains(bundle.TuKhoa));
            }

            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(c => c.NgayBatDau);
            var result = await query
            .Skip((bundle.PageIndex - 1) * bundle.PageSize)
            .Take(bundle.PageSize)
            .Select(x => new ModelChimuc()
            {
                Id = x.Id,
                Ten = x.Ten,
                NgayBatDau = x.NgayBatDau,
                NgayKetThuc = x.NgayKetThuc,
                SoNgayLamViec = x.SoNgayLamViec
            }).ToListAsync();

            var pagedResult = new PagedResult<ModelChimuc>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = result
            };
            return new ApiSuccessResult<PagedResult<ModelChimuc>>(pagedResult);
        }

        private int TinhToanSoNgayLamViec(DateTime ngayBD, DateTime ngayKT, string ngayNghiMD, DateTime[] ngayNghiTC = null)
        {
            string[] ngayMD = ngayNghiMD.Split(';');
            List<string> listNgayNghiMD = new List<string>();
            List<DateTime> listNgayNghiTC = new List<DateTime>();

            if (!String.IsNullOrEmpty(ngayNghiMD))
            {
                foreach (var item in ngayMD)
                {
                    if (item != null)
                    {
                        switch (item)
                        {
                            case "2":
                                listNgayNghiMD.Add("Monday");
                                break;

                            case "3":
                                listNgayNghiMD.Add("Tuesday");
                                break;

                            case "4":
                                listNgayNghiMD.Add("Wednesday");
                                break;

                            case "5":
                                listNgayNghiMD.Add("Thursday");
                                break;

                            case "6":
                                listNgayNghiMD.Add("Friday");
                                break;

                            case "7":
                                listNgayNghiMD.Add("Saturday");
                                break;

                            case "1":
                                listNgayNghiMD.Add("Sunday");
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            if (ngayNghiTC != null)
            {
                // loại bỏ trùng ngày nghỉ tự chọn với ngày nghỉ mặc định
                foreach (var item in ngayNghiTC)
                {
                    var chuyenNgay = (DateTime)item;
                    var thu = chuyenNgay.DayOfWeek.ToString();
                    bool bang = false;
                    if (!String.IsNullOrEmpty(ngayNghiMD))
                    {
                        foreach (var el in listNgayNghiMD)
                        {
                            if (el == thu)
                            {
                                bang = true;
                                break;
                            }
                        }
                    }

                    if (!bang)
                    {
                        listNgayNghiTC.Add(chuyenNgay);
                    }
                }
            }

            // loại bỏ phần tử trùng nhau trong list:
            listNgayNghiTC = listNgayNghiTC.Distinct().ToList();

            // tính số ngày làm việc
            int soNgayLamViec = 0;
            while (ngayBD <= ngayKT)
            {
                var kiemTra = ngayBD.DayOfWeek.ToString();
                if (!String.IsNullOrEmpty(ngayNghiMD))
                {
                    foreach (var item in listNgayNghiMD)
                    {
                        if (item == kiemTra)
                        {
                            soNgayLamViec = soNgayLamViec - 1;
                            break;
                        }
                    }
                }
                if (ngayNghiTC != null)
                {
                    foreach (var el in listNgayNghiTC)
                    {
                        if (el.Date == ngayBD.Date)
                        {
                            soNgayLamViec = soNgayLamViec - 1;
                            break;
                        }
                    }
                }

                ngayBD = ngayBD.AddDays(1);
                soNgayLamViec++;
            }

            return soNgayLamViec;
        }

        public async Task<ApiResult<ChiMucTheoId>> GetChiMucTheoId(int id)
        {
            var index = await _context.ChiMucs.Include(x => x.NgayNghiTuChons).FirstOrDefaultAsync(x => x.Id == id);
            if (index == null)
            {
                return new ApiErrorResult<ChiMucTheoId>("Không tồn tại id");
            }
            var result = new ChiMucTheoId()
            {
                Id = index.Id,
                Ten = index.Ten,
                NgayKetThuc = index.NgayKetThuc.ToString("MM/dd/yyyy"),
                NgayBatDau = index.NgayBatDau.ToString("MM/dd/yyyy"),
                NgayNghiMacDinh = index.NgayNghiMacDinh.Split(';')
            };

            if (index.NgayNghiTuChons != null)
            {
                result.NgayNghiTuChon = new List<NgayTuChonModel>();

                foreach (var item in index.NgayNghiTuChons)
                {
                    result.NgayNghiTuChon.Add(new NgayTuChonModel()
                    {
                        Id = item.Id,
                        Ngay = item.NgayNghi.ToString("yyyy-MM-dd")
                    });
                }
            }
            return new ApiErrorResult<ChiMucTheoId>(result);
        }

        public async Task<ApiResult<bool>> iNgayTonTai(DateTime ngay, int? id)
        {
            var chiMuc = _context.ChiMucs;
            bool ketQua;
            if (id == null)
            {
                ketQua = await chiMuc.AnyAsync(x => (x.NgayBatDau <= ngay) && (x.NgayKetThuc >= ngay));
            }
            else
            {
                ketQua = await chiMuc.AnyAsync(x => (x.NgayBatDau <= ngay)
                                && (x.NgayKetThuc >= ngay)
                                && x.Id != id);
            }

            if (ketQua)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>();
        }

        public async Task<ApiResult<UpdateChiMucModel>> Update(CapNhatChiMuc bundle)
        {
            var index = await _context.ChiMucs.FindAsync(bundle.Id);
            if (index == null)
            {
                return new ApiErrorResult<UpdateChiMucModel>("Không tìm thấy Id");
            }

            // cập nhật lại ngày tự chọn
            if (bundle.NgayTuChonCapNhat != null)
            {
                List<NgayNghiTuChon> ngayTCList = new List<NgayNghiTuChon>();
                int i = 0;
                foreach (var item in bundle.NgayTuChonCapNhat)
                {
                    var ngayTC = await _context.NgayNghiTuChons.FindAsync(bundle.IdNgayTuChon[i]);
                    if (ngayTC == null)
                    {
                        return new ApiErrorResult<UpdateChiMucModel>("Không tìm thấy Id");
                    }
                    ngayTC.NgayNghi = item;
                    ngayTCList.Add(ngayTC);
                    i++;
                }
                _context.NgayNghiTuChons.UpdateRange(ngayTCList);
            }

            // thêm ngày tự chọn
            if (bundle.NgayNghiTuChon != null)
            {
                List<NgayNghiTuChon> ngayTCThemList = new List<NgayNghiTuChon>();
                foreach (var item in bundle.NgayNghiTuChon)
                {
                    var themNgay = new NgayNghiTuChon()
                    {
                        IdChiMuc = bundle.Id,
                        NgayNghi = item
                    };
                    ngayTCThemList.Add(themNgay);
                }
                _context.NgayNghiTuChons.AddRange(ngayTCThemList);
            }

            string ngayMD = "";
            if (bundle.NgayNghiMacDinhCapNhat != null)
            {
                foreach (var item in bundle.NgayNghiMacDinhCapNhat)
                {
                    ngayMD = ngayMD + item + ";";
                }
            }

            int soNgayLamViec = 0;
            if (bundle.NgayNghiTuChon != null && bundle.NgayTuChonCapNhat != null)
            {
                DateTime[] listNgayNghi = new DateTime[bundle.NgayNghiTuChon.Length + bundle.NgayTuChonCapNhat.Length];
                bundle.NgayNghiTuChon.CopyTo(listNgayNghi, 0);
                bundle.NgayTuChonCapNhat.CopyTo(listNgayNghi, bundle.NgayNghiTuChon.Length);
                soNgayLamViec = this.TinhToanSoNgayLamViec(index.NgayBatDau, index.NgayKetThuc, ngayMD, listNgayNghi);
            }
            else
            {
                if (bundle.NgayTuChonCapNhat != null && bundle.NgayNghiTuChon == null)
                {
                    soNgayLamViec = this.TinhToanSoNgayLamViec(index.NgayBatDau, index.NgayKetThuc, ngayMD, bundle.NgayTuChonCapNhat);
                }
                else
                {
                    if (bundle.NgayNghiTuChon != null && bundle.NgayTuChonCapNhat == null)
                    {
                        soNgayLamViec = this.TinhToanSoNgayLamViec(index.NgayBatDau, index.NgayKetThuc, ngayMD, bundle.NgayNghiTuChon);
                    }
                }
            }

            if (bundle.NgayNghiTuChon == null && bundle.NgayTuChonCapNhat == null)
            {
                soNgayLamViec = this.TinhToanSoNgayLamViec(index.NgayBatDau, index.NgayKetThuc, ngayMD);
            }
            index.NgayNghiMacDinh = ngayMD;
            index.SoNgayLamViec = soNgayLamViec;
            _context.ChiMucs.Update(index);

            await _context.SaveChangesAsync();

            var result = new UpdateChiMucModel()
            {
                Id = index.Id,
                SoNgayLamViec = index.SoNgayLamViec
            };
            return new ApiSuccessResult<UpdateChiMucModel>(result);
        }

        public async Task<ApiResult<int>> iNgayLamViec()
        {
            var ngayHienTai = DateTime.Now;

            var chiMuc = _context.ChiMucs
                .Where(x => (x.NgayBatDau.Date <= ngayHienTai.Date) && (x.NgayKetThuc.Date >= ngayHienTai.Date));

            var demSo = await chiMuc.CountAsync();
            if (demSo < 1)
            {
                return new ApiErrorResult<int>("Ngày không tồn tại");
            }

            var ketQua = await chiMuc.FirstOrDefaultAsync();
            string[] ngayMD = ketQua.NgayNghiMacDinh.Split(';');
            List<string> listNgayNghiMD = new List<string>();

            if (!String.IsNullOrEmpty(ketQua.NgayNghiMacDinh))
            {
                foreach (var item in ngayMD)
                {
                    if (item != null)
                    {
                        switch (item)
                        {
                            case "2":
                                listNgayNghiMD.Add("Monday");
                                break;

                            case "3":
                                listNgayNghiMD.Add("Tuesday");
                                break;

                            case "4":
                                listNgayNghiMD.Add("Wednesday");
                                break;

                            case "5":
                                listNgayNghiMD.Add("Thursday");
                                break;

                            case "6":
                                listNgayNghiMD.Add("Friday");
                                break;

                            case "7":
                                listNgayNghiMD.Add("Saturday");
                                break;

                            case "1":
                                listNgayNghiMD.Add("Sunday");
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            // kiểm tra ngày hiện tại có trùng với ngày thứ không
            var thu = ngayHienTai.DayOfWeek.ToString();
            foreach (var item in listNgayNghiMD)
            {
                if (item == thu)
                {
                    return new ApiErrorResult<int>("Ngày nghỉ");
                }
            }

            // kiểm tra ngày hiện tại có trùng với ngày nghỉ tự chọn không
            var ngayTC = await _context.NgayNghiTuChons.Where(x => x.IdChiMuc == ketQua.Id).ToListAsync();
            foreach (var item in ngayTC)
            {
                if (item.NgayNghi.Date == ngayHienTai.Date)
                {
                    return new ApiErrorResult<int>("Ngày nghỉ");
                }
            }

            return new ApiSuccessResult<int>(ketQua.Id);
        }
    }
}