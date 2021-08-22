using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.SanPhamApiClient;
using QuanLyKho.ViewModels.SanPham;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class SanPhamsController : BaseController
    {
        private readonly ISanPhamApiClient _sanPhamApiClient;

        public SanPhamsController(ISanPhamApiClient sanPhamApiClient)
        {
            _sanPhamApiClient = sanPhamApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, int idLoaiSanPham = 0, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetSanPhamPaging()
            {
                TuKhoa = tuKhoa,
                IdLoaiSanPham = idLoaiSanPham,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _sanPhamApiClient.GetSanPhamPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowAllSanPham(int id)
        {
            var resultId = await _sanPhamApiClient.GetShowAllSanPham(id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _sanPhamApiClient.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetNhacNho(int id)
        {
            var resultId = await _sanPhamApiClient.GetNhacNho(id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUpdateSanPham(int id)
        {
            var resultId = await _sanPhamApiClient.GetByUpdateSanPham(id);
            return Ok(resultId);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] TaoToanBoSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _sanPhamApiClient.Create(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sanPhamApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] CapNhatToanBoSanPham bundle)
        {
            var result = await _sanPhamApiClient.Update(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNhacNho(NhacNhoSPModel bundle)
        {
            var result = await _sanPhamApiClient.UpdateNhacNho(bundle);
            return Ok(result);
        }
    }
}