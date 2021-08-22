using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.NhaCungCapApiClient;
using QuanLyKho.ViewModels.NhaCungCap;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class NhaCungCapsController : BaseController
    {
        private readonly INhaCungCapApiClient _nhaCungCapApiClient;

        public NhaCungCapsController(INhaCungCapApiClient nhaCungCapApiClient)
        {
            _nhaCungCapApiClient = nhaCungCapApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetNhaCungCapPagingRequest()
            {
                TuKhoa = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _nhaCungCapApiClient.GetNhaCungCapPaging(bundle);

            ViewBag.Keyword = keyword;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> iMaThue(string thue, int? id)
        {
            var resultId = await _nhaCungCapApiClient.iMaThue(thue, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> iEmail(string email, int? id)
        {
            var resultId = await _nhaCungCapApiClient.iEmail(email, id);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var nhaCC = await _nhaCungCapApiClient.GetById(id);
            return Ok(nhaCC);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _nhaCungCapApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatNhaCungCap request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhaCungCapApiClient.Update(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoNhaCungCap request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _nhaCungCapApiClient.Create(request);
            return Ok(result);
        }
    }
}