using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.LoaiSanPhamService;
using QuanLyKho.ViewModels.LoaiSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly ILoaiSanPhamService _loaiSanPhamService;

        public LoaiSanPhamController(ILoaiSanPhamService loaiSanPhamService)
        {
            _loaiSanPhamService = loaiSanPhamService;
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _loaiSanPhamService.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetLoaiSanPham request)
        {
            var result = await _loaiSanPhamService.GetLoaiSanPhamPaging(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _loaiSanPhamService.GetById(id);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetTenLoaiSP()
        {
            var result = await _loaiSanPhamService.GetTenLoaiSP();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoLoaiSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiSanPhamService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loaiSanPhamService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatLoaiSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiSanPhamService.Update(request.Id, request);
            return Ok(result);
        }
    }
}