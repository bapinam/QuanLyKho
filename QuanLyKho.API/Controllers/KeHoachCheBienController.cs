using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Service.KeHoachCheBienService;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.ViewModels.KeHoachCheBien;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeHoachCheBienController : ControllerBase
    {
        private readonly IKeHoachCheBienService _keHoachCheBienService;

        public KeHoachCheBienController(IKeHoachCheBienService keHoachCheBienService)
        {
            _keHoachCheBienService = keHoachCheBienService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllKeHoach([FromQuery] GetAllKeHoachCheBien bundle)
        {
            var result = await _keHoachCheBienService.GetAllKeHoach(bundle);
            return Ok(result);
        }

        [HttpGet("paging/nhan-kehoach")]
        public async Task<IActionResult> NhanKeHoachCheBien([FromQuery] GetAllKeHoachCheBien bundle, string userName)
        {
            var result = await _keHoachCheBienService.NhanKeHoachCheBien(bundle, userName);
            return Ok(result);
        }

        [HttpGet("theo-thang")]
        public async Task<IActionResult> GetAllKeHoachTheoThang(int thang, int nam)
        {
            var result = await _keHoachCheBienService.GetAllKeHoachTheoThang(thang, nam);
            return Ok(result);
        }

        [HttpGet("nhan-vien")]
        public async Task<IActionResult> NhanVienKHCheBien(string tuKhoa)
        {
            var result = await _keHoachCheBienService.NhanVienKHCheBien(tuKhoa);
            return Ok(result);
        }

        [HttpGet("congthuc-sanpham")]
        public async Task<IActionResult> GetCongThucSanPham(string tuKhoa, int idLoaiSanPham)
        {
            var result = await _keHoachCheBienService.GetCongThucSanPham(tuKhoa, idLoaiSanPham);
            return Ok(result);
        }

        [HttpGet("loai-sanpham/{id}")]
        public async Task<IActionResult> GetLoaiSanPhamKH(int id)
        {
            var result = await _keHoachCheBienService.GetLoaiSanPhamKH(id);
            return Ok(result);
        }

        [HttpGet("xem-kehoach/{id}")]
        public async Task<IActionResult> GetInfoKeHoachById(int id)
        {
            var result = await _keHoachCheBienService.GetInfoKeHoachById(id);
            return Ok(result);
        }

        [HttpGet("xem-donvitinh/{idSanPham}")]
        public async Task<IActionResult> GetShowDonViTinhKHCB(int idSanPham)
        {
            var result = await _keHoachCheBienService.GetShowDonViTinhKHCB(idSanPham);
            return Ok(result);
        }

        [HttpGet("donvitinh/{idSanPham}")]
        public async Task<IActionResult> GetDonViTinhKHCB(int idSanPham)
        {
            var result = await _keHoachCheBienService.GetDonViTinhKHCB(idSanPham);
            return Ok(result);
        }

        [HttpGet("nhom-sanpham")]
        public async Task<IActionResult> GetNhomSanPhamKH()
        {
            var result = await _keHoachCheBienService.GetNhomSanPhamKH();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoKeHoachCheBien bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachCheBienService.Create(bundle);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatKeHoachCheBien bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _keHoachCheBienService.Update(bundle);
            return Ok(result);
        }

        [HttpPut("nhan-kehoach/{id}")]
        public async Task<IActionResult> UpdateNhanKeHoach(long id)
        {
            var result = await _keHoachCheBienService.UpdateNhanKeHoach(id);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _keHoachCheBienService.Delete(id);
            return Ok(result);
        }

        [HttpDelete("cong-thuc/{id}")]
        public async Task<IActionResult> DeleteCongThuc(long id)
        {
            var result = await _keHoachCheBienService.DeleteCongThuc(id);
            return Ok(result);
        }
    }
}