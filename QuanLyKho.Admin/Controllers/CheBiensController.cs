using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.CheBienApiClient;
using QuanLyKho.ApiIntegration.ChiMucApiClient;
using QuanLyKho.ViewModels.CheBien;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class CheBiensController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;
        private readonly ICheBienApiClient _cheBienApiClient;

        public CheBiensController(IChiMucApiClient chiMucApiClient, ICheBienApiClient cheBienApiClient)
        {
            _chiMucApiClient = chiMucApiClient;
            _cheBienApiClient = cheBienApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, string ngay, int trangThai = 0, int pageIndex = 1,
            int pageSize = 10)
        {
            var chiMuc = await _chiMucApiClient.iNgayLamViec();
            if (!chiMuc.IsSuccessed)
            {
                return RedirectToAction("ChiMuc", "Home");
            }
            var bundle = new TimKiemPhieuCheBien()
            {
                TuKhoa = tuKhoa,
                Ngay = ngay,
                TrangThai = trangThai,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _cheBienApiClient.GetAllPhieuCheBien(bundle);
            ViewBag.ChiMuc = chiMuc.ResultObj;
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKeHoachCheBienChuaHoanThanh(TimKiemCheBien bundle)
        {
            var result = await _cheBienApiClient.GetAllKeHoachCheBienChuaHoanThanh(bundle);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> HuyChiTietCheBien(long id)
        {
            var result = await _cheBienApiClient.HuyChiTietCheBien(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDanhSachChebien(long id)
        {
            var result = await _cheBienApiClient.GetAllDanhSachChebien(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCongThucKeHoach(long id)
        {
            var result = await _cheBienApiClient.GetCongThucKeHoach(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhieuCheBienDangCho(TimKiemCheBien bundle)
        {
            var result = await _cheBienApiClient.GetPhieuCheBienDangCho(bundle);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetIdInforCheBien(long id)
        {
            var result = await _cheBienApiClient.GetIdInforCheBien(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> HuyPhieuCheBien(long id, string nguoiDuyet)
        {
            var result = await _cheBienApiClient.HuyPhieuCheBien(id, nguoiDuyet);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> CapNhatTrangThaiCheBien(long id, string userName)
        {
            var result = await _cheBienApiClient.CapNhatTrangThaiCheBien(id, userName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoPhieuCheBien bundle)
        {
            var result = await _cheBienApiClient.Create(bundle);
            return Ok(result);
        }
    }
}