using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.NhaCungCapService;
using QuanLyKho.ViewModels.NhaCungCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaCungCapController : ControllerBase
    {
        private readonly INhaCungCapService _nhaCungCapService;

        public NhaCungCapController(INhaCungCapService nhaCungCapService)
        {
            _nhaCungCapService = nhaCungCapService;
        }

        [HttpGet("kiemtra-sothue")]
        public async Task<IActionResult> iMaThue(string thue, int? id)
        {
            var resultId = await _nhaCungCapService.iMaThue(thue, id);
            return Ok(resultId);
        }

        [HttpGet("kiemtra-email")]
        public async Task<IActionResult> iEmail(string email, int? id)
        {
            var resultId = await _nhaCungCapService.iEmail(email, id);
            return Ok(resultId);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetNhaCungCapPagingRequest request)
        {
            var result = await _nhaCungCapService.GetNhaCungCapPaging(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _nhaCungCapService.GetById(id);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nhaCungCapService.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatNhaCungCap request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhaCungCapService.Update(request.Id, request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoNhaCungCap request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhaCungCapService.Create(request);
            return Ok(result);
        }
    }
}