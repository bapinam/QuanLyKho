using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.CheBienService;
using QuanLyKho.ViewModels.CheBien;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheBienController : ControllerBase
    {
        private readonly ICheBienService _cheBienService;

        public CheBienController(ICheBienService cheBienService)
        {
            _cheBienService = cheBienService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPhieuCheBien([FromQuery] TimKiemPhieuCheBien bundle)
        {
            var result = await _cheBienService.GetAllPhieuCheBien(bundle);
            return Ok(result);
        }

        [HttpGet("ke-hoach")]
        public async Task<IActionResult> GetAllKeHoachCheBienChuaHoanThanh([FromQuery] TimKiemCheBien bundle)
        {
            var result = await _cheBienService.GetAllKeHoachCheBienChuaHoanThanh(bundle);
            return Ok(result);
        }

        [HttpGet("cong-thuc/{id}")]
        public async Task<IActionResult> GetCongThucKeHoach(long id)
        {
            var result = await _cheBienService.GetCongThucKeHoach(id);
            return Ok(result);
        }

        [HttpGet("huy/{id}/{nguoiDuyet}")]
        public async Task<IActionResult> HuyPhieuCheBien(long id, string nguoiDuyet)
        {
            var result = await _cheBienService.HuyPhieuCheBien(id, nguoiDuyet);
            return Ok(result);
        }

        [HttpGet("huy-chebien/{id}")]
        public async Task<IActionResult> HuyChiTietCheBien(long id)
        {
            var result = await _cheBienService.HuyChiTietCheBien(id);
            return Ok(result);
        }

        [HttpGet("danh-sach-chebien/{id}")]
        public async Task<IActionResult> GetAllDanhSachChebien(long id)
        {
            var result = await _cheBienService.GetAllDanhSachChebien(id);
            return Ok(result);
        }

        [HttpGet("cap-nhat/{id}/{userName}")]
        public async Task<IActionResult> CapNhatTrangThaiCheBien(long id, string userName)
        {
            var result = await _cheBienService.CapNhatTrangThaiCheBien(id, userName);
            return Ok(result);
        }

        [HttpGet("phieu-chebien")]
        public async Task<IActionResult> GetPhieuCheBienDangCho([FromQuery] TimKiemCheBien bundle)
        {
            var result = await _cheBienService.GetPhieuCheBienDangCho(bundle);
            return Ok(result);
        }

        [HttpGet("chi-tiet-phieu-chebien/{id}")]
        public async Task<IActionResult> GetIdInforCheBien(long id)
        {
            var result = await _cheBienService.GetIdInforCheBien(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoPhieuCheBien bundle)
        {
            var result = await _cheBienService.Create(bundle);
            return Ok(result);
        }
    }
}