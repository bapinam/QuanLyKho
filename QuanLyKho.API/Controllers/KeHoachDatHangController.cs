using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.KeHoachDatHangService;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.ViewModels.KeHoachDatHang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeHoachDatHangController : ControllerBase
    {
        private readonly IKeHoachDatHangService _keHoachDatHangService;

        public KeHoachDatHangController(IKeHoachDatHangService keHoachDatHangService)
        {
            _keHoachDatHangService = keHoachDatHangService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllKeHoach([FromQuery] GetAllKeHoachDatHang bundle)
        {
            var result = await _keHoachDatHangService.GetAllKeHoach(bundle);
            return Ok(result);
        }

        [HttpGet("kehoach-dukien")]
        public async Task<IActionResult> KeHoachCheBienDuKien()
        {
            var result = await _keHoachDatHangService.KeHoachCheBienDuKien();
            return Ok(result);
        }

        [HttpGet("du-kien/{id}")]
        public async Task<IActionResult> GetInfoKeHoachCheBienDuKien(long id)
        {
            var result = await _keHoachDatHangService.GetInfoKeHoachCheBienDuKien(id);
            return Ok(result);
        }

        [HttpGet("paging/nhan-kehoach")]
        public async Task<IActionResult> NhanKeHoachDatHang([FromQuery] GetAllKeHoachDatHang bundle, string userName)
        {
            var result = await _keHoachDatHangService.NhanKeHoachDatHang(bundle, userName);
            return Ok(result);
        }

        [HttpGet("loai-nguyenvatlieu/{nhomNVL}")]
        public async Task<IActionResult> GetLoaiNVLKH(int nhomNVL)
        {
            var result = await _keHoachDatHangService.GetLoaiNVLKH(nhomNVL);
            return Ok(result);
        }

        [HttpGet("nguyen-vat-lieu")]
        public async Task<IActionResult> GetNguyenVatLieuKH(int idLoaiNVL, string tuKhoa)
        {
            var result = await _keHoachDatHangService.GetNguyenVatLieuKH(idLoaiNVL, tuKhoa);
            return Ok(result);
        }

        [HttpGet("xem-donvitinh/{idNVL}")]
        public async Task<IActionResult> GetShowDonViTinhKHDH(int idNVL)
        {
            var result = await _keHoachDatHangService.GetShowDonViTinhKHDH(idNVL);
            return Ok(result);
        }

        [HttpGet("donvitinh/{idNVL}")]
        public async Task<IActionResult> GetDonViTinhKHDH(int idNVL)
        {
            var result = await _keHoachDatHangService.GetDonViTinhKHDH(idNVL);
            return Ok(result);
        }

        [HttpGet("nhan-vien")]
        public async Task<IActionResult> NhanVienKHDatHang(string tuKhoa)
        {
            var result = await _keHoachDatHangService.NhanVienKHDatHang(tuKhoa);
            return Ok(result);
        }

        [HttpGet("theo-thang")]
        public async Task<IActionResult> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var result = await _keHoachDatHangService.GetAllKeHoachTheoThang(thang, nam);
            return Ok(result);
        }

        [HttpGet("xem-kehoach/{id}")]
        public async Task<IActionResult> GetInfoKeHoachById(int id)
        {
            var result = await _keHoachDatHangService.GetInfoKeHoachById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoKeHoachDatHang bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachDatHangService.Create(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatKeHoachDatHang bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachDatHangService.Update(bundle);
            return Ok(result);
        }

        [HttpPut("nhan-kehoach/{id}")]
        public async Task<IActionResult> UpdateNhanKeHoach(long id)
        {
            var result = await _keHoachDatHangService.UpdateNhanKeHoach(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _keHoachDatHangService.Delete(id);
            return Ok(result);
        }

        [HttpDelete("nguyen-vat-lieu/{id}")]
        public async Task<IActionResult> DeleteCongThuc(int id)
        {
            var result = await _keHoachDatHangService.DeleteNguyenVatLieu(id);
            return Ok(result);
        }
    }
}