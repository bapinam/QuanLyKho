using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.ChiMucApiClient;
using QuanLyKho.ApiIntegration.KeHoachDatHangApiClient;
using QuanLyKho.ViewModels.KeHoachDatHang;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class NhiemVuKeHoachDatHangController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;
        private readonly IKeHoachDatHangApiClient _keHoachApiClient;

        public NhiemVuKeHoachDatHangController(IChiMucApiClient chiMucApiClient, IKeHoachDatHangApiClient keHoachApiClient)
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
            var bundle = new GetAllKeHoachDatHang()
            {
                TuKhoa = tuKhoa,
                NgayDuKien = ngayDuKien,
                TrangThai = trangThai,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _keHoachApiClient.NhanKeHoachDatHang(bundle, userName);
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