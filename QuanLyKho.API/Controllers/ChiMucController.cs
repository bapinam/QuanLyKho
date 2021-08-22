using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Service.ChiMucService;
using QuanLyKho.ViewModels.ChiMuc;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiMucController : ControllerBase
    {
        private readonly IChiMucService _chiMucService;

        public ChiMucController(IChiMucService chiMucService)
        {
            _chiMucService = chiMucService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllChiMuc index)
        {
            var result = await _chiMucService.GetAll(index);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChiMucTheoId(int id)
        {
            var result = await _chiMucService.GetChiMucTheoId(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> iNgayLamViec()
        {
            var result = await _chiMucService.iNgayLamViec();
            return Ok(result);
        }

        [HttpGet("ten/{ten}")]
        public async Task<IActionResult> GetName(string ten)
        {
            var result = await _chiMucService.GetName(ten);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoChiMuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _chiMucService.Create(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatChiMuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _chiMucService.Update(bundle);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _chiMucService.Delete(id);
            return Ok(result);
        }

        [HttpDelete("ngay-nghi/{id}")]
        public async Task<IActionResult> DeleteNgayNghi(int id)
        {
            var result = await _chiMucService.DeleteNgayNghi(id);
            return Ok(result);
        }
    }
}