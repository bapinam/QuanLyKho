using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.ChiMucApiClient;
using QuanLyKho.ApiIntegration.DatHangApiClient;
using QuanLyKho.ViewModels.DatHang;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class DatHangsController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;
        private readonly IDatHangApiClient _datHangApiClient;

        public DatHangsController(IChiMucApiClient chiMucApiClient, IDatHangApiClient datHangApiClient)
        {
            _chiMucApiClient = chiMucApiClient;
            _datHangApiClient = datHangApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, string ngay, int pageIndex = 1, int pageSize = 10)
        {
            var chiMuc = await _chiMucApiClient.iNgayLamViec();
            if (!chiMuc.IsSuccessed)
            {
                return RedirectToAction("ChiMuc", "Home");
            }

            var bundle = new TimKiemPhieuNhapDaTra()
            {
                TuKhoa = tuKhoa,
                Ngay = ngay,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _datHangApiClient.GetAllPhieuNhapDaTra(bundle);
            ViewBag.ChiMuc = chiMuc.ResultObj;
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKeHoachDatHangChuaHoanThanh(TimKiemDatHang bundle)
        {
            var result = await _datHangApiClient.GetAllKeHoachDatHangChuaHoanThanh(bundle);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTienDaThanhToan(long id)
        {
            var result = await _datHangApiClient.GetTienDaThanhToan(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDanhSachDatHang(long id)
        {
            var result = await _datHangApiClient.GetDanhSachDatHang(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> NhaCungCapDatHang(string tuKhoa)
        {
            var result = await _datHangApiClient.NhaCungCapDatHang(tuKhoa);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> HuyChiTietDatHang(long id)
        {
            var result = await _datHangApiClient.HuyChiTietDatHang(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateThanhToan(long id, string tienDaTra)
        {
            var result = await _datHangApiClient.UpdateThanhToan(id, tienDaTra);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetChiTietPhieuNhap(long id)
        {
            var result = await _datHangApiClient.GetChiTietPhieuNhap(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPhieuNhapChuaTra(TimKiemDatHang bundle)
        {
            var result = await _datHangApiClient.GetAllPhieuNhapChuaTra(bundle);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NhapHoaDon bundle)
        {
            var result = await _datHangApiClient.Create(bundle);
            return Ok(result);
        }
    }
}