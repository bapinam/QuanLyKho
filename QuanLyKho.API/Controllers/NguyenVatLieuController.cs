using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.NguyenVatLieuService;
using QuanLyKho.ViewModels.NguyenVatLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguyenVatLieuController : ControllerBase
    {
        private readonly INguyenVatLieuService _nguyenVatLieuService;

        public NguyenVatLieuController(INguyenVatLieuService nguyenVatLieuService)
        {
            _nguyenVatLieuService = nguyenVatLieuService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetNguyenVatLieuPaging([FromQuery] GetNguyenVatLieuPaging request)
        {
            var result = await _nguyenVatLieuService.GetNguyenVatLieuPaging(request);
            return Ok(result);
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _nguyenVatLieuService.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet("cap-nhat/{id}")]
        public async Task<IActionResult> GetByUpdateNguyenVatLieu(int id)
        {
            var resultId = await _nguyenVatLieuService.GetByUpdateNguyenVatLieu(id);
            return Ok(resultId);
        }

        [HttpGet("xem/{id}")]
        public async Task<IActionResult> GetShowAllNguyenVatLieu(int id)
        {
            var resultId = await _nguyenVatLieuService.GetShowAllNguyenVatLieu(id);
            return Ok(resultId);
        }

        [HttpGet("nhac-nho/{id}")]
        public async Task<IActionResult> GetNhacNho(int id)
        {
            var resultId = await _nguyenVatLieuService.GetNhacNho(id);
            return Ok(resultId);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoNguyenVatLieu request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nguyenVatLieuService.Create(request);
            return Ok(result);
        }

        [HttpPut("hinh-anh/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateHinAnh(int id, [FromForm] CapNhatHinhAnhNVL request)
        {
            var result = await _nguyenVatLieuService.CapNhatHinhAnh(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nguyenVatLieuService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatNguyenVatLieu bundle)
        {
            var result = await _nguyenVatLieuService.Update(bundle);
            return Ok(result);
        }

        [HttpPut("nhac-nho")]
        public async Task<IActionResult> UpdateNhacNho(NhacNhoModel bundle)
        {
            var result = await _nguyenVatLieuService.UpdateNhacNho(bundle);
            return Ok(result);
        }
    }
}