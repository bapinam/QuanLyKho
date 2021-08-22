using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.DatHangService;
using QuanLyKho.ViewModels.DatHang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatHangController : ControllerBase
    {
        private readonly IDatHangService _datHangService;

        public DatHangController(IDatHangService datHangService)
        {
            _datHangService = datHangService;
        }

        [HttpGet("ke-hoach")]
        public async Task<IActionResult> GetAllKeHoachDatHangChuaHoanThanh([FromQuery] TimKiemDatHang bundle)
        {
            var result = await _datHangService.GetAllKeHoachDatHangChuaHoanThanh(bundle);
            return Ok(result);
        }

        [HttpGet("phieu-chuatra")]
        public async Task<IActionResult> GetAllPhieuNhapChuaTra([FromQuery] TimKiemDatHang bundle)
        {
            var result = await _datHangService.GetAllPhieuNhapChuaTra(bundle);
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPhieuNhapDaTra([FromQuery] TimKiemPhieuNhapDaTra bundle)
        {
            var result = await _datHangService.GetAllPhieuNhapDaTra(bundle);
            return Ok(result);
        }

        [HttpGet("danh-sach-dathang/{id}")]
        public async Task<IActionResult> GetDanhSachDatHang(long id)
        {
            var result = await _datHangService.GetDanhSachDatHang(id);
            return Ok(result);
        }

        [HttpGet("lay-tien-da-thanhtoan/{id}")]
        public async Task<IActionResult> GetTienDaThanhToan(long id)
        {
            var result = await _datHangService.GetTienDaThanhToan(id);
            return Ok(result);
        }

        [HttpGet("chi-tiet-phieunhap/{id}")]
        public async Task<IActionResult> GetChiTietPhieuNhap(long id)
        {
            var result = await _datHangService.GetChiTietPhieuNhap(id);
            return Ok(result);
        }

        [HttpGet("huy-chitiet-dathang/{id}")]
        public async Task<IActionResult> HuyChiTietDatHang(long id)
        {
            var result = await _datHangService.HuyChiTietDatHang(id);
            return Ok(result);
        }

        [HttpGet("nha-cung-cap/{tuKhoa}")]
        public async Task<IActionResult> NhaCungCapDatHang(string tuKhoa)
        {
            var result = await _datHangService.NhaCungCapDatHang(tuKhoa);
            return Ok(result);
        }

        [HttpGet("cap-nhat-thanhtoan/{id}/{tienDaTra}")]
        public async Task<IActionResult> UpdateThanhToan(long id, string tienDaTra)
        {
            var result = await _datHangService.UpdateThanhToan(id, tienDaTra);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NhapHoaDon bundle)
        {
            var result = await _datHangService.Create(bundle);
            return Ok(result);
        }
    }
}