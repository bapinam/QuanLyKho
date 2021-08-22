using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.LoaiNguyenVatLieuApiClient;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.LoaiNguyenVatLieu;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class LoaiNguyenVatLieusController : BaseController
    {
        private readonly ILoaiNguyenVatLieuApiClient _loaiNguyenVatLieuApiClient;

        public LoaiNguyenVatLieusController(ILoaiNguyenVatLieuApiClient loaiNguyenVatLieuApiClient)
        {
            _loaiNguyenVatLieuApiClient = loaiNguyenVatLieuApiClient;
        }

        public async Task<IActionResult> Index(NhomLoaiNVL? nhomLoai, string tuKhoa, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetLoaiNguyenVatLieu()
            {
                TuKhoa = tuKhoa,
                PageIndex = pageIndex,
                NhomLoai = nhomLoai,
                PageSize = pageSize
            };

            var data = await _loaiNguyenVatLieuApiClient.GetLoaiNguyenVatLieuPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _loaiNguyenVatLieuApiClient.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _loaiNguyenVatLieuApiClient.GetById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTenLoaiNVL()
        {
            var result = await _loaiNguyenVatLieuApiClient.GetTenLoaiNVL();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoLoaiNguyenVatLieu request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiNguyenVatLieuApiClient.Create(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _loaiNguyenVatLieuApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatLoaiNguyenVatLieu request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _loaiNguyenVatLieuApiClient.Update(request);
            return Ok(result);
        }
    }
}