using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.LoaiNguyenVatLieuService;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiNguyenVatLieuController : ControllerBase
    {
        private readonly ILoaiNguyenVatLieuService _loaiNguyenVatLieuService;

        public LoaiNguyenVatLieuController(ILoaiNguyenVatLieuService loaiNguyenVatLieuService)
        {
            _loaiNguyenVatLieuService = loaiNguyenVatLieuService;
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _loaiNguyenVatLieuService.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetLoaiNguyenVatLieu request)
        {
            var result = await _loaiNguyenVatLieuService.GetLoaiNguyenVatLieuPaging(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _loaiNguyenVatLieuService.GetById(id);
            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetTenLoaiNVL()
        {
            var result = await _loaiNguyenVatLieuService.GetTenLoaiNVL();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoLoaiNguyenVatLieu request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiNguyenVatLieuService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loaiNguyenVatLieuService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatLoaiNguyenVatLieu request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiNguyenVatLieuService.Update(request.Id, request);
            return Ok(result);
        }
    }
}