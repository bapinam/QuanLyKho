using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.ThongBaoService;
using QuanLyKho.ViewModels.ThongBao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongBaoController : ControllerBase
    {
        private readonly IThongBaoService _thongBaoService;

        public ThongBaoController(IThongBaoService thongBaoService)
        {
            _thongBaoService = thongBaoService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAll([FromQuery] SetThongBao index)
        {
            var result = await _thongBaoService.GetThongBaoPaging(index);
            return Ok(result);
        }

        [HttpGet("get-top/{userName}")]
        public async Task<IActionResult> GetTopThongBao(string userName)
        {
            var result = await _thongBaoService.GetTopThongBao(userName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoThongBao bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _thongBaoService.Create(bundle);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _thongBaoService.Delete(id);
            return Ok(result);
        }

        [HttpDelete("delete-all")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAll()
        {
            var result = await _thongBaoService.DeleteAll();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDaXem(long id)
        {
            var result = await _thongBaoService.UpdateDaXem(id);
            return Ok(result);
        }
    }
}