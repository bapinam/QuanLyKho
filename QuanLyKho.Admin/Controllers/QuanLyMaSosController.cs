using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.QuanLyMaSoApiClient;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.QuanLyMaSo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class QuanLyMaSosController : Controller
    {
        private readonly IQuanLyMaSoApiClient _quanLyMaSoApiClient;

        public QuanLyMaSosController(IQuanLyMaSoApiClient quanLyMaSoApiClient)
        {
            _quanLyMaSoApiClient = quanLyMaSoApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(MaLoai type)
        {
            var result = await _quanLyMaSoApiClient.GetAll(type);
            return Ok(result);
        }

        [HttpGet]
        public async Task<bool> iTen(string ten, int? id)
        {
            var data = await _quanLyMaSoApiClient.iTen(ten, id);
            return data.IsSuccessed;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoMaSo bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _quanLyMaSoApiClient.Create(bundle);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _quanLyMaSoApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatMaSo bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _quanLyMaSoApiClient.Update(bundle);
            return Ok(result);
        }
    }
}