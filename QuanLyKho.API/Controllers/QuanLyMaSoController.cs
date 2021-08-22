using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Service.QuanLyMaSoService;
using QuanLyKho.ViewModels.QuanLyMaSo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyMaSoController : ControllerBase
    {
        private readonly IQuanLyMaSoService _quanLyMaSoService;

        public QuanLyMaSoController(IQuanLyMaSoService indexLimitService)
        {
            _quanLyMaSoService = indexLimitService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(MaLoai loai)
        {
            var result = await _quanLyMaSoService.GetAll(loai);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoMaSo bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _quanLyMaSoService.Create(bundle);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _quanLyMaSoService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatMaSo bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _quanLyMaSoService.Update(bundle);
            return Ok(result);
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var result = await _quanLyMaSoService.iTen(ten, id);
            return Ok(result);
        }
    }
}