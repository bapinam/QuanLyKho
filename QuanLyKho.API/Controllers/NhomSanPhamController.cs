using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.NhomSanPhamService;
using QuanLyKho.ViewModels.NhomSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhomSanPhamController : ControllerBase
    {
        private readonly INhomSanPhamService _nhomSanPhamService;

        public NhomSanPhamController(INhomSanPhamService nhomSanPhamService)
        {
            _nhomSanPhamService = nhomSanPhamService;
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _nhomSanPhamService.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetNhomSanPham request)
        {
            var result = await _nhomSanPhamService.GetNhomSanPhamPaging(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _nhomSanPhamService.GetById(id);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetTenNhomSP()
        {
            var result = await _nhomSanPhamService.GetTenNhomSP();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoNhomSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhomSanPhamService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nhomSanPhamService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatNhomSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhomSanPhamService.Update(request.Id, request);
            return Ok(result);
        }
    }
}