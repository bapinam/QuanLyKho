using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.SanPhamService;
using QuanLyKho.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService _sanPhamService;

        public SanPhamController(ISanPhamService sanPhamService)
        {
            _sanPhamService = sanPhamService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetSanPhamPaging([FromQuery] GetSanPhamPaging request)
        {
            var result = await _sanPhamService.GetSanPhamPaging(request);
            return Ok(result);
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _sanPhamService.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet("cap-nhat/{id}")]
        public async Task<IActionResult> GetByUpdateSanPham(int id)
        {
            var resultId = await _sanPhamService.GetByUpdateSanPham(id);
            return Ok(resultId);
        }

        [HttpGet("xem/{id}")]
        public async Task<IActionResult> GetShowAllSanPham(int id)
        {
            var resultId = await _sanPhamService.GetShowAllSanPham(id);
            return Ok(resultId);
        }

        [HttpGet("nhac-nho/{id}")]
        public async Task<IActionResult> GetNhacNho(int id)
        {
            var resultId = await _sanPhamService.GetNhacNho(id);
            return Ok(resultId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sanPhamService.Create(request);
            return Ok(result);
        }

        [HttpPut("hinh-anh/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateHinAnh(int id, [FromForm] CapNhatHinhAnhSP request)
        {
            var result = await _sanPhamService.CapNhatHinhAnh(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sanPhamService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatSanPham bundle)
        {
            var result = await _sanPhamService.Update(bundle);
            return Ok(result);
        }

        [HttpPut("nhac-nho")]
        public async Task<IActionResult> UpdateNhacNho(NhacNhoSPModel bundle)
        {
            var result = await _sanPhamService.UpdateNhacNho(bundle);
            return Ok(result);
        }
    }
}