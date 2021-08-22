using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.ThongBaoApiClient;
using QuanLyKho.ViewModels.ThongBao;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class ThongBaosController : BaseController
    {
        private readonly IThongBaoApiClient _thongBaoApiClient;

        public ThongBaosController(IThongBaoApiClient thongBaoApiClient)
        {
            _thongBaoApiClient = thongBaoApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, string ngayNhan, int trangThai = 1, int pageIndex = 1,
            int pageSize = 10)
        {
            var userName = User.Identity.Name;
            var bundle = new SetThongBao()
            {
                TuKhoa = tuKhoa,
                NgayNhanThongBao = ngayNhan,
                TrangThaiXem = trangThai,
                UserName = userName,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _thongBaoApiClient.GetThongBaoPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetTopThongBao()
        {
            var userName = User.Identity.Name;
            var result = await _thongBaoApiClient.GetTopThongBao(userName);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _thongBaoApiClient.Delete(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDaXem(long id)
        {
            var result = await _thongBaoApiClient.UpdateDaXem(id);
            return Ok(result);
        }
    }
}