using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.LoaiSanPhamApiClient;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.LoaiSanPham;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class LoaiSanPhamsController : BaseController
    {
        private readonly ILoaiSanPhamApiClient _loaiSanPhamApiClient;

        public LoaiSanPhamsController(ILoaiSanPhamApiClient loaiSanPhamApiClient)
        {
            _loaiSanPhamApiClient = loaiSanPhamApiClient;
        }

        public async Task<IActionResult> Index(int idNhomLoai, string tuKhoa, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetLoaiSanPham()
            {
                TuKhoa = tuKhoa,
                IdNhomLoai = idNhomLoai,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _loaiSanPhamApiClient.GetLoaiSanPhamPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _loaiSanPhamApiClient.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _loaiSanPhamApiClient.GetById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenLoaiSP()
        {
            var result = await _loaiSanPhamApiClient.GetTenLoaiSP();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoLoaiSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiSanPhamApiClient.Create(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loaiSanPhamApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatLoaiSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiSanPhamApiClient.Update(request);
            return Ok(result);
        }
    }
}