using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.ChiMucApiClient;
using QuanLyKho.ViewModels.ChiMuc;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    [Authorize]
    public class ChiMucsController : BaseController
    {
        private readonly IChiMucApiClient _chiMucApiClient;

        public ChiMucsController(IChiMucApiClient chiMucApiClient)
        {
            _chiMucApiClient = chiMucApiClient;
        }

        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var bundle = new GetAllChiMuc()
            {
                TuKhoa = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _chiMucApiClient.GetAll(bundle);

            ViewBag.Keyword = keyword;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetChiMucTheoId(int id)
        {
            var result = await _chiMucApiClient.GetChiMucTheoId(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetName(string ten)
        {
            var result = await _chiMucApiClient.GetName(ten);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoChiMuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _chiMucApiClient.Create(bundle);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _chiMucApiClient.Delete(id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNgayNghi(int id)
        {
            var result = await _chiMucApiClient.DeleteNgayNghi(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatChiMuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _chiMucApiClient.Update(bundle);
            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}