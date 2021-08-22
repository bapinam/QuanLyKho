using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.NguyenVatLieuApiClient;
using QuanLyKho.ViewModels.NguyenVatLieu;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class NguyenVatLieusController : BaseController
    {
        private readonly INguyenVatLieuApiClient _nguyenVatLieuApiClient;

        public NguyenVatLieusController(INguyenVatLieuApiClient nguyenVatLieuApiClient)
        {
            _nguyenVatLieuApiClient = nguyenVatLieuApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, int idLoaiNguyenVatLieu = 0, int pageIndex = 1,
            int pageSize = 10)
        {
            var bundle = new GetNguyenVatLieuPaging()
            {
                TuKhoa = tuKhoa,
                IdLoaiNguyenVatLieu = idLoaiNguyenVatLieu,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _nguyenVatLieuApiClient.GetNguyenVatLieuPaging(bundle);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetShowAllNguyenVatLieu(int id)
        {
            var resultId = await _nguyenVatLieuApiClient.GetShowAllNguyenVatLieu(id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int? id)
        {
            var resultId = await _nguyenVatLieuApiClient.iTen(ten, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetNhacNho(int id)
        {
            var resultId = await _nguyenVatLieuApiClient.GetNhacNho(id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetByUpdateNguyenVatLieu(int id)
        {
            var resultId = await _nguyenVatLieuApiClient.GetByUpdateNguyenVatLieu(id);
            return Ok(resultId);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] TaoToanBoNguyenVatLieu request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nguyenVatLieuApiClient.Create(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nguyenVatLieuApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update([FromForm] CapNhatToanBoNguyenVatLieu bundle)
        {
            var result = await _nguyenVatLieuApiClient.Update(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNhacNho(NhacNhoModel bundle)
        {
            var result = await _nguyenVatLieuApiClient.UpdateNhacNho(bundle);
            return Ok(result);
        }
    }
}