using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.NhomSanPhamApiClient;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.NhomSanPham;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class NhomSanPhamsController : BaseController
    {
        private readonly INhomSanPhamApiClient _nhomSanPhamApiClient;

        public NhomSanPhamsController(INhomSanPhamApiClient nhomSanPhamApiClient)
        {
            _nhomSanPhamApiClient = nhomSanPhamApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetNhomSanPham()
            {
                TuKhoa = tuKhoa,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _nhomSanPhamApiClient.GetNhomSanPhamPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _nhomSanPhamApiClient.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _nhomSanPhamApiClient.GetById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenNhomSP()
        {
            var result = await _nhomSanPhamApiClient.GetTenNhomSP();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoNhomSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhomSanPhamApiClient.Create(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nhomSanPhamApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatNhomSanPham request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhomSanPhamApiClient.Update(request);
            return Ok(result);
        }
    }
}