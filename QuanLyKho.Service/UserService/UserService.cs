using AutoMapper;
using QuanLyKho.Data.Entities;
using QuanLyKho.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.Date.EF;
using QuanLyKho.Service.Common;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.User;

namespace QuanLyKho.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly QuanLyKhoDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _storageService;

        // private IRoleService _roleService;
        private const string PRODUCT_CONTENT_FOLDER_NAME = SystemConstants.ImageFolder;

        public UserService(QuanLyKhoDbContext context,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            // IRoleService roleService,
            IConfiguration config, IMapper mapper,
         IFileStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _mapper = mapper;
            _storageService = storageService;
            // _roleService = roleService;
        }

        // Đăng nhập vào hệ thống
        public async Task<ApiResult<string>> Authencate(LoginRequest bundle)
        {
            if (bundle.UserName == null || bundle.Password == null)
            {
                return new ApiErrorResult<string>("Không được bỏ trống dữ liệu");
            }

            var user = await _userManager.FindByNameAsync(bundle.UserName);
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");

            // username, password, rememberme, true: login sai quá số lần sẽ bị khóa
            var result = await _signInManager.PasswordSignInAsync(user, bundle.Password, bundle.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Nhập mật khẩu sai");
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.DaNghiViec)
            {
                return new ApiErrorResult<string>("Bạn không thể đăng nhập, vì bạn đã nghỉ việc");
            }

            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(SystemConstants.Avatar, user.HinhAnh!= null ? user.HinhAnh : "OK")
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
                var identityRole = await _roleManager.FindByNameAsync(role);
                var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
                if (roleClaims != null && roleClaims.Any())
                {
                    claims = claims.Concat(await _roleManager.GetClaimsAsync(identityRole)).ToList();
                }
            }

            // sau khi có claims thì mã khóa claims  bằng Symmetric
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[SystemConstants.Token.Key]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config[SystemConstants.Token.Issuer],
                _config[SystemConstants.Token.Issuer],
                claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: creds);

            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest bundle)
        {
            var query = _userManager.Users.Where(x => x.LoaiTaiKhoan == LoaiTaiKhoan.NhanVien);
            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(x => x.MaSo.Contains(bundle.TuKhoa) || x.CanCuocCongDan.Contains(bundle.TuKhoa));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((bundle.PageIndex - 1) * bundle.PageSize)
                .Take(bundle.PageSize)
                .OrderByDescending(x => x.MaSo)
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    CanCuocCongDan = x.CanCuocCongDan,
                    Ho = x.Ho,
                    Ten = x.Ten,
                    GioiTinh = x.GioiTinh == true ? "Nam" : "Nữ",
                    NgaySinh = x.NgaySinh,
                    DiaChi = x.DiaChi,
                    TinhTrangLamViec = x.TinhTrangLamViec == TinhTrangLamViec.ChuaCoViec ? "Chưa có việc làm"
                                : x.TinhTrangLamViec == TinhTrangLamViec.DangLamViec ? "Đang làm việc" : "Đã nghỉ việc"
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<UserVm>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
        }

        public async Task<ApiResult<Guid>> Register(RegisterRequest bundle)
        {
            var code = await _context.QuanLyMaSos.FirstOrDefaultAsync(x => x.Ten == bundle.MaSo);
            var stt = 1;
            ViTri:
            var location = code.ViTri + stt;

            var str = code.Ten + location;

            var check = await _userManager.Users.AnyAsync(x => x.MaSo == str);
            if (check)
            {
                stt++;
                goto ViTri;
            }
            var userName = str;
            var passWord = "@Mk" + str;
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                goto ViTri;
            }

            code.ViTri = location;
            _context.QuanLyMaSos.Update(code);
            await _context.SaveChangesAsync();

            user = _mapper.Map(bundle, user);
            user.MaSo = userName;
            user.UserName = userName;
            user.TinhTrangLamViec = TinhTrangLamViec.ChuaCoViec;
            user.PhoneNumber = bundle.SoDienThoai;
            user.LoaiTaiKhoan = LoaiTaiKhoan.NhanVien;
            var result = await _userManager.CreateAsync(user, passWord);

            if (result.Succeeded)
            {
                return new ApiSuccessResult<Guid>(user.Id);
            }
            return new ApiErrorResult<Guid>("Đăng ký không thành công");
        }

        public async Task<ApiResult<bool>> iCard(string card, Guid? id)
        {
            if (id != null)
            {
                if (await _userManager.Users.AnyAsync(x => x.CanCuocCongDan == card && x.Id != id))
                {
                    return new ApiErrorResult<bool>("Căn cước đã tồn tại tài khoản");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                if (await _userManager.Users.AnyAsync(x => x.CanCuocCongDan == card))
                {
                    return new ApiErrorResult<bool>("Căn cước đã tồn tại tài khoản");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> iEmail(string email, Guid? id)
        {
            if (id != null)
            {
                if (await _userManager.Users.AnyAsync(x => x.Email == email && x.Id != id))
                {
                    return new ApiErrorResult<bool>("Email đã tồn tại tài khoản");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
            else
            {
                if (await _userManager.Users.AnyAsync(x => x.Email == email))
                {
                    return new ApiErrorResult<bool>("Email đã tồn tại tài khoản");
                }
                else
                {
                    return new ApiSuccessResult<bool>();
                }
            }
        }

        public async Task<ApiResult<bool>> iEmailName(string email, string name)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == email && x.UserName != name))
            {
                return new ApiErrorResult<bool>("Email đã tồn tại tài khoản");
            }
            else
            {
                return new ApiSuccessResult<bool>();
            }
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest bundle)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var list = _mapper.Map(bundle, user);

            var result = await _userManager.UpdateAsync(list);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<ApiResult<bool>> UpdateInfor(UpdateInfor bundle)
        {
            var user = await _userManager.FindByIdAsync(bundle.Id.ToString());

            var list = _mapper.Map(bundle, user);
            list.PhoneNumber = bundle.SoDienThoai;
            var result = await _userManager.UpdateAsync(list);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }

        public async Task<ApiResult<bool>> UpdatePassword(UserUpdatePassword bundle)
        {
            var user = await _userManager.FindByNameAsync(bundle.Ten);
            if (user == null) return new ApiErrorResult<bool>("Tài khoản không tồn tại");

            var result = await _signInManager.PasswordSignInAsync(user, bundle.MatKhauCu, false, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<bool>("Nhập mật khẩu hiện tại sai");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var update = await _userManager.ResetPasswordAsync(user, token, bundle.XacNhanMatKhau);
            if (update.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Mật khẩu phải có ít nhất ký tự đặc biệt và chữ hoa");
        }

        public async Task<ApiResult<GetByIdListUser>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<GetByIdListUser>("Người dùng không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<GetByIdListUser>(user);
            if (user.GioiTinh)
            {
                userVm.GioiTinh = "Nam";
            }
            else
            {
                userVm.GioiTinh = "Nữ";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.ChuaCoViec)
            {
                userVm.TinhTrangLamViec = "Chưa làm việc";
            }
            else
            {
                if (user.TinhTrangLamViec == TinhTrangLamViec.DangLamViec)
                {
                    userVm.TinhTrangLamViec = " Đang làm việc";
                }
                else
                {
                    userVm.TinhTrangLamViec = "Đã nghỉ việc";
                }
            }

            var date = user.NgaySinh.ToString("dd-MM-yyyy");
            userVm.NgaySinhCapNhat = date;
            var date2 = user.NgaySinh.ToString("yyyy-MM-dd");
            userVm.NgaySinh = date2;
            userVm.SoDienThoai = user.PhoneNumber;
            userVm.DuongDanHinhAnh = user.HinhAnh;
            return new ApiSuccessResult<GetByIdListUser>(userVm);
        }

        public async Task<ApiResult<UserNameVm>> GetByUserName(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserNameVm>("Người dùng không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<UserNameVm>(user);
            userVm.Password = "@Mk" + user.MaSo;
            if (user.GioiTinh)
            {
                userVm.GioiTinh = "Nam";
            }
            else
            {
                userVm.GioiTinh = "Nữ";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.ChuaCoViec)
            {
                userVm.TinhTrangLamViec = "Chưa có việc làm";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.DangLamViec)
            {
                userVm.TinhTrangLamViec = "Đang làm việc";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.DaNghiViec)
            {
                userVm.TinhTrangLamViec = "Đã nghỉ việc";
            }
            userVm.Id = id;
            userVm.SoDienThoai = user.PhoneNumber;
            return new ApiSuccessResult<UserNameVm>(userVm);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Người dùng không tồn tại");
            }

            var role = await _userManager.GetRolesAsync(user);

            foreach (var item in role)
            {
                if (await _userManager.IsInRoleAsync(user, item))
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }
            }
            if (user.HinhAnh != null)
            {
                await _storageService.DeleteFileAsync(user.HinhAnh);
            }

            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<ApiResult<string>> UpdateJobStauts(UpdateJobStauts bundle)
        {
            var user = await _userManager.FindByIdAsync(bundle.Id.ToString());

            user.TinhTrangLamViec = bundle.TinhTrangLamViec;
            await _userManager.UpdateAsync(user);

            string js = "";
            if (user.TinhTrangLamViec == TinhTrangLamViec.ChuaCoViec)
            {
                js = "Chưa có việc làm";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.DangLamViec)
            {
                js = "Đang làm việc";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.DaNghiViec)
            {
                js = "Đã nghỉ việc";
            }

            return new ApiSuccessResult<string>(js);
        }

        public async Task<ApiResult<GetByIdListUser>> GetByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user == null)
            {
                return new ApiErrorResult<GetByIdListUser>("Người dùng không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = _mapper.Map<GetByIdListUser>(user);
            if (user.GioiTinh)
            {
                userVm.GioiTinh = "Nam";
            }
            else
            {
                userVm.GioiTinh = "Nữ";
            }

            if (user.TinhTrangLamViec == TinhTrangLamViec.ChuaCoViec)
            {
                userVm.TinhTrangLamViec = "Chưa làm việc";
            }
            else
            {
                if (user.TinhTrangLamViec == TinhTrangLamViec.DangLamViec)
                {
                    userVm.TinhTrangLamViec = " Đang làm việc";
                }
                else
                {
                    userVm.TinhTrangLamViec = "Đã nghỉ việc";
                }
            }

            var date = user.NgaySinh.ToString("dd-MM-yyyy");
            userVm.NgaySinhCapNhat = date;
            var date2 = user.NgaySinh.ToString("yyyy-MM-dd");
            userVm.NgaySinh = date2;
            userVm.Id = user.Id;
            userVm.SoDienThoai = user.PhoneNumber;
            userVm.DuongDanHinhAnh = user.HinhAnh;
            return new ApiSuccessResult<GetByIdListUser>(userVm);
        }

        public async Task<ApiResult<string>> UpdateImage(UpdateImageUser bundle)
        {
            var user = await _userManager.FindByNameAsync(bundle.Ten);

            if (bundle.File != null)
            {
                if (user.HinhAnh != null)
                {
                    await _storageService.DeleteFileAsync(user.HinhAnh);
                }
            }

            // lưu lại ảnh mới
            user.HinhAnh = await this.SaveFile(bundle.File);
            await _userManager.UpdateAsync(user);
            return new ApiSuccessResult<string>(user.HinhAnh);
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + PRODUCT_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<ApiResult<string>> GetImage(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            if (user.HinhAnh != null)
            {
                return new ApiSuccessResult<string>(user.HinhAnh);
            }
            else
            {
                return new ApiErrorResult<string>();
            }
        }

        public async Task<ApiResult<bool>> ResetPassWord(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("Tài khoản không tồn tại");
            }

            var pass = "@Mk" + user.MaSo;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var update = await _userManager.ResetPasswordAsync(user, token, pass);

            if (update.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật thất bại");
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUserRole(GetUserPagingRequest bundle)
        {
            var query = _userManager.Users.Where(x => x.LoaiTaiKhoan == LoaiTaiKhoan.NhanVien);
            if (!string.IsNullOrEmpty(bundle.TuKhoa))
            {
                query = query.Where(x => x.MaSo.Contains(bundle.TuKhoa) || x.CanCuocCongDan.Contains(bundle.TuKhoa));
            }

            //3. Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((bundle.PageIndex - 1) * bundle.PageSize)
                .Take(bundle.PageSize)
                .OrderByDescending(x => x.MaSo)
                .Select(x => new UserVm()
                {
                    Id = x.Id,
                    MaSo = x.MaSo,
                    CanCuocCongDan = x.CanCuocCongDan,
                    Ho = x.Ho,
                    Ten = x.Ten,
                    DiaChi = x.DiaChi,
                    TinhTrangLamViec = x.TinhTrangLamViec == TinhTrangLamViec.ChuaCoViec ? "Chưa có việc làm"
                                : x.TinhTrangLamViec == TinhTrangLamViec.DangLamViec ? "Đang làm việc" : "Đã nghỉ việc"
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<UserVm>()
            {
                TotalRecords = totalRow,
                PageIndex = bundle.PageIndex,
                PageSize = bundle.PageSize,
                Items = data
            };
            return new ApiSuccessResult<PagedResult<UserVm>>(pagedResult);
        }

        public async Task<List<VaiTroVM>> GetRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            var role = await _userManager.GetRolesAsync(user);

            List<VaiTroVM> result = new List<VaiTroVM>();
            foreach (var item in role)
            {
                var r = _roleManager.Roles.Where(x => x.Name == item);

                var vaiTro = await
                    r.Select(x => new VaiTroVM()
                    {
                        Id = x.Id,
                        TenVaiTro = x.Name
                    }).FirstOrDefaultAsync(); ;

                result.Add(vaiTro);
            }

            return new List<VaiTroVM>(result);
        }

        public async Task<ApiResult<bool>> GiaoQuyenHan(GiaoQuyenHanNguoiDung bundle)
        {
            var user = await _userManager.FindByIdAsync(bundle.Id.ToString());

            var role = await _userManager.GetRolesAsync(user);
            // chưa giao quyền
            if (role.Count == 0)
            {
                // thêm quyền
                if (bundle.QuanLy != null)
                {
                    foreach (var tenQuyen in bundle.QuanLy)
                    {
                        await _userManager.AddToRoleAsync(user, tenQuyen);
                    }
                }
            }
            // đã giao quyền
            else
            {
                // xóa tắt cả quyền
                foreach (var item in role)
                {
                    await _userManager.RemoveFromRoleAsync(user, item);
                }

                // thêm lại quyền
                if (bundle.QuanLy != null)
                {
                    foreach (var tenQuyen in bundle.QuanLy)
                    {
                        await _userManager.AddToRoleAsync(user, tenQuyen);
                    }
                }
            }

            return new ApiSuccessResult<bool>();
        }
    }
}