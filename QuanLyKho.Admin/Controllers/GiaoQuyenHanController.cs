using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.UserApiClient;
using QuanLyKho.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class GiaoQuyenHanController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public GiaoQuyenHanController(IUserApiClient userApiClient)
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
            var data = await _userApiClient.GetUserRole(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var user = await _userApiClient.GetRole(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> GiaoQuyenHan(GiaoQuyenHanNguoiDung bundle)
        {
            var result = await _userApiClient.GiaoQuyenHan(bundle);
            return Ok(result);
        }
    }
}