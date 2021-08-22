using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.Service.CongThucService;
using QuanLyKho.ViewModels.CongThuc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongThucController : ControllerBase
    {
        private readonly ICongThucService _congThucService;

        public CongThucController(ICongThucService congThucService)
        {
            _congThucService = congThucService;
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetCongThucSPPaging request)
        {
            var result = await _congThucService.GetCongThucSPPaging(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetListCongThuc(int id)
        {
            var result = await _congThucService.GetListCongThuc(id);
            return Ok(result);
        }

        [HttpGet("kiemtra-ten")]
        public async Task<IActionResult> iTen(string ten, int idSP, int? id)
        {
            var result = await _congThucService.iTen(ten, idSP, id);
            return Ok(result);
        }

        [HttpGet("kiemtra-nguyenvatlieu")]
        public async Task<IActionResult> iNguyenVatLieu(int idCongThuc, int idNguyenVatLieu)
        {
            var result = await _congThucService.iNguyenVatLieu(idCongThuc, idNguyenVatLieu);
            return Ok(result);
        }

        [HttpGet("loai-nguyenvatlieu")]
        public async Task<IActionResult> GetLoaiNguyenVatLieu(NhomLoaiNVL? group)
        {
            var result = await _congThucService.GetLoaiNguyenVatLieu(group);
            return Ok(result);
        }

        [HttpGet("nguyenvatlieu")]
        public async Task<IActionResult> GetListNguyenVatLieu([FromQuery] ListBundleNguyenVatLieu bundle)
        {
            var result = await _congThucService.GetListNguyenVatLieu(bundle);
            return Ok(result);
        }

        [HttpGet("donvitinh/{id}")]
        public async Task<IActionResult> GetListDonViTinhs(int id)
        {
            var result = await _congThucService.GetListDonViTinhs(id);
            return Ok(result);
        }

        [HttpGet("congthuc/{id}")]
        public async Task<IActionResult> GetListIdCongThuc(int id)
        {
            var result = await _congThucService.GetListIdCongThuc(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoCongThuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucService.Create(bundle);
            return Ok(result);
        }

        [HttpPut("dinh-dau")]
        public async Task<IActionResult> UpdateDinhDauCongThuc([FromBody] CapNhatDinhDauCongThuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucService.UpdateDinhDauCongThuc(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CapNhatCongThuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucService.Update(bundle);
            return Ok(result);
        }

        [HttpDelete("{id}/{idSanPham}")]
        public async Task<IActionResult> Delete(int id, int idSanPham)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucService.Delete(id, idSanPham);
            return Ok(result);
        }

        [HttpDelete("nguyenvatlieu/{id}")]
        public async Task<IActionResult> DeleteNguyenVatLieu(long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucService.DeleteNguyenVatLieu(id);
            return Ok(result);
        }
    }
}