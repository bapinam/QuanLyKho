using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.UserApiClient;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.User;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserApiClient _userApiClient;

        public UsersController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetUserPagingRequest()
            {
                TuKhoa = tuKhoa,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUsersPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public async Task<ApiResult<UserNameVm>> Create(RegisterRequest bundle)
        {
            var result = await _userApiClient.RegisterUser(bundle);
            return result;
        }

        [HttpGet]
        public async Task<bool> iCard(string card, Guid? id)
        {
            var data = await _userApiClient.iCard(card, id);
            return data.IsSuccessed;
        }

        [HttpGet]
        public async Task<bool> iEmail(string email, Guid? id)
        {
            var data = await _userApiClient.iEmail(email, id);
            return data.IsSuccessed;
        }

        [HttpGet]
        public async Task<bool> iEmailName(string email, string name)
        {
            var data = await _userApiClient.iEmailName(email, name);
            return data.IsSuccessed;
        }

        [HttpGet]
        public async Task<GetByIdListUser> GetById(Guid id)
        {
            var user = await _userApiClient.GetById(id);

            var data = new GetByIdListUser()
            {
                MaSo = user.ResultObj.MaSo,
                CanCuocCongDan = user.ResultObj.CanCuocCongDan,
                Ten = user.ResultObj.Ten,
                Ho = user.ResultObj.Ho,
                UserName = user.ResultObj.UserName,
                SoDienThoai = user.ResultObj.SoDienThoai,
                NgaySinhCapNhat = user.ResultObj.NgaySinhCapNhat,
                NgaySinh = user.ResultObj.NgaySinh,
                GioiTinh = user.ResultObj.GioiTinh,
                DiaChi = user.ResultObj.DiaChi,
                Email = user.ResultObj.Email,
                TinhTrangLamViec = user.ResultObj.TinhTrangLamViec,
                DuongDanHinhAnh = user.ResultObj.DuongDanHinhAnh
            };
            return data;
        }

        [HttpGet]
        public async Task<GetByIdListUser> GetByName(string name)
        {
            var user = await _userApiClient.GetByName(name);

            var data = new GetByIdListUser()
            {
                Id = user.ResultObj.Id,
                MaSo = user.ResultObj.MaSo,
                CanCuocCongDan = user.ResultObj.CanCuocCongDan,
                Ten = user.ResultObj.Ten,
                Ho = user.ResultObj.Ho,
                UserName = user.ResultObj.UserName,
                SoDienThoai = user.ResultObj.SoDienThoai,
                NgaySinhCapNhat = user.ResultObj.NgaySinhCapNhat,
                NgaySinh = user.ResultObj.NgaySinh,
                GioiTinh = user.ResultObj.GioiTinh,
                DiaChi = user.ResultObj.DiaChi,
                Email = user.ResultObj.Email,
                TinhTrangLamViec = user.ResultObj.TinhTrangLamViec,
                DuongDanHinhAnh = user.ResultObj.DuongDanHinhAnh
            };
            return data;
        }

        [HttpPut]
        public async Task<ApiResult<bool>> Update(UserUpdateRequest bundle)
        {
            var data = await _userApiClient.UpdateUser(bundle.Id, bundle);
            return data;
        }

        [HttpPut]
        public async Task<ApiResult<bool>> UpdateInfor(UpdateInfor bundle)
        {
            var data = await _userApiClient.UpdateInfor(bundle);
            return data;
        }

        [HttpDelete]
        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var data = await _userApiClient.Delete(id);
            return data;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateJobStauts(UpdateJobStauts bundle)
        {
            var result = await _userApiClient.UpdateJobStauts(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword(UserUpdatePassword bundle)
        {
            var result = await _userApiClient.UpdatePassword(bundle);
            return Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateImage([FromForm] UpdateImageUser bundle)
        {
            var result = await _userApiClient.UpdateImage(bundle);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetImage(string name)
        {
            var result = await _userApiClient.GetImage(name);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassWord(Guid id)
        {
            var result = await _userApiClient.ResetPassWord(id);
            return Ok(result);
        }
    }
}