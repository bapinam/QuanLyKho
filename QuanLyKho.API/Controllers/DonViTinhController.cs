using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Service.DonViTinhService;
using QuanLyKho.ViewModels.DonViTinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonViTinhController : ControllerBase
    {
        private readonly IDonViTinhService _donViTinhService;

        public DonViTinhController(IDonViTinhService donViTinhService)
        {
            _donViTinhService = donViTinhService;
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int id, long? idDVT, bool loaiDongGoi)
        {
            var resultId = await _donViTinhService.iTen(ten, id, idDVT, loaiDongGoi);
            return Ok(resultId);
        }

        [HttpGet("{id}/{loaiDongGoi}")]
        public async Task<IActionResult> GetDonViTinh(int id, LoaiDongGoi loaiDongGoi)
        {
            var result = await _donViTinhService.GetDonViTinh(id, loaiDongGoi);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoDonViTinh request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _donViTinhService.Create(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _donViTinhService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatDonViTinh request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _donViTinhService.Update(request);
            return Ok(result);
        }
    }
}