using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.ChiMucApiClient;
using QuanLyKho.ApiIntegration.KeHoachCheBienApiClient;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class NhiemVuKeHoachCheBienController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;
        private readonly IKeHoachCheBienApiClient _keHoachApiClient;

        public NhiemVuKeHoachCheBienController(IChiMucApiClient chiMucApiClient, IKeHoachCheBienApiClient keHoachApiClient)
        {
            _chiMucApiClient = chiMucApiClient;
            _keHoachApiClient = keHoachApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, string ngayDuKien, int trangThai = 0, int pageIndex = 1,
            int pageSize = 10)
        {
            var chiMuc = await _chiMucApiClient.iNgayLamViec();
            if (!chiMuc.IsSuccessed)
            {
                return RedirectToAction("ChiMuc", "Home");
            }
            var userName = User.Identity.Name;
            var bundle = new GetAllKeHoachCheBien()
            {
                TuKhoa = tuKhoa,
                NgayDuKien = ngayDuKien,
                TrangThai = trangThai,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _keHoachApiClient.NhanKeHoachCheBien(bundle, userName);
            ViewBag.ChiMuc = chiMuc.ResultObj;
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateNhanKeHoach(long id)
        {
            var result = await _keHoachApiClient.UpdateNhanKeHoach(id);
            return Ok(result);
        }
    }
}