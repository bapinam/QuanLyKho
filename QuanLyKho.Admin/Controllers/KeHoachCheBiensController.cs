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
    public class KeHoachCheBiensController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;
        private readonly IKeHoachCheBienApiClient _keHoachApiClient;

        public KeHoachCheBiensController(IChiMucApiClient chiMucApiClient, IKeHoachCheBienApiClient keHoachApiClient)
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
            var bundle = new GetAllKeHoachCheBien()
            {
                TuKhoa = tuKhoa,
                NgayDuKien = ngayDuKien,
                TrangThai = trangThai,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _keHoachApiClient.GetAllKeHoach(bundle);
            ViewBag.ChiMuc = chiMuc.ResultObj;
            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var result = await _keHoachApiClient.GetAllKeHoachTheoThang(thang, nam);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetInfoKeHoachById(int id)
        {
            var result = await _keHoachApiClient.GetInfoKeHoachById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> NhanVienKHCheBien(string tuKhoa)
        {
            var result = await _keHoachApiClient.NhanVienKHCheBien(tuKhoa);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowDonViTinhKHCB(int idSanPham)
        {
            var result = await _keHoachApiClient.GetShowDonViTinhKHCB(idSanPham);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDonViTinhKHCB(int idSanPham)
        {
            var result = await _keHoachApiClient.GetDonViTinhKHCB(idSanPham);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCongThucSanPham(string tuKhoa, int idLoaiSanPham)
        {
            var result = await _keHoachApiClient.GetCongThucSanPham(tuKhoa, idLoaiSanPham);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiSanPhamKH(int id)
        {
            var result = await _keHoachApiClient.GetLoaiSanPhamKH(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetNhomSanPhamKH()
        {
            var result = await _keHoachApiClient.GetNhomSanPhamKH();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoKeHoachCheBien bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachApiClient.Create(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatKeHoachCheBien bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachApiClient.Update(bundle);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _keHoachApiClient.Delete(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCongThuc(long id)
        {
            var result = await _keHoachApiClient.DeleteCongThuc(id);
            return Ok(result);
        }
    }
}