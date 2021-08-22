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
    public class KeHoachDatHangsController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;
        private readonly IKeHoachDatHangApiClient _keHoachDatHangApiClient;

        public KeHoachDatHangsController(IChiMucApiClient chiMucApiClient, IKeHoachDatHangApiClient keHoachApiClient)
        {
            _chiMucApiClient = chiMucApiClient;
            _keHoachDatHangApiClient = keHoachApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, string ngayDuKien, int trangThai = 0, int pageIndex = 1,
            int pageSize = 10)
        {
            var chiMuc = await _chiMucApiClient.iNgayLamViec();
            if (!chiMuc.IsSuccessed)
            {
                return RedirectToAction("ChiMuc", "Home");
            }
            var bundle = new GetAllKeHoachDatHang()
            {
                TuKhoa = tuKhoa,
                NgayDuKien = ngayDuKien,
                TrangThai = trangThai,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _keHoachDatHangApiClient.GetAllKeHoach(bundle);
            ViewBag.ChiMuc = chiMuc.ResultObj;
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> KeHoachCheBienDuKien()
        {
            var result = await _keHoachDatHangApiClient.KeHoachCheBienDuKien();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoKeHoachCheBienDuKien(long id)
        {
            var result = await _keHoachDatHangApiClient.GetInfoKeHoachCheBienDuKien(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiNVLKH(int nhomNVL)
        {
            var result = await _keHoachDatHangApiClient.GetLoaiNVLKH(nhomNVL);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNguyenVatLieuKH(int idLoaiNVL, string tuKhoa)
        {
            var result = await _keHoachDatHangApiClient.GetNguyenVatLieuKH(idLoaiNVL, tuKhoa);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowDonViTinhKHDH(int idNVL)
        {
            var result = await _keHoachDatHangApiClient.GetShowDonViTinhKHDH(idNVL);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDonViTinhKHDH(int idNVL)
        {
            var result = await _keHoachDatHangApiClient.GetDonViTinhKHDH(idNVL);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> NhanVienKHDatHang(string tuKhoa)
        {
            var result = await _keHoachDatHangApiClient.NhanVienKHDatHang(tuKhoa);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var result = await _keHoachDatHangApiClient.GetAllKeHoachTheoThang(thang, nam);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoKeHoachById(int id)
        {
            var result = await _keHoachDatHangApiClient.GetInfoKeHoachById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoKeHoachDatHang bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachDatHangApiClient.Create(bundle);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _keHoachDatHangApiClient.Delete(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCongThuc(int id)
        {
            var result = await _keHoachDatHangApiClient.DeleteNguyenVatLieu(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatKeHoachDatHang bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachDatHangApiClient.Update(bundle);
            return Ok(result);
        }
    }
}