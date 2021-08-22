using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.DonViTinhApiClient;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.DonViTinh;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class DonViTinhsController : BaseController
    {
        private readonly IDonViTinhApiClient _donViTinhApiClient;

        public DonViTinhsController(IDonViTinhApiClient donViTinhService)
        {
            _donViTinhApiClient = donViTinhService;
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int id, long? idDVT, bool loaiDongGoi)
        {
            var resultId = await _donViTinhApiClient.iTen(ten, id, idDVT, loaiDongGoi);
            return Ok(resultId);
        }

        [HttpGet]
        public async Task<IActionResult> GetDonViTinh(int id, LoaiDongGoi loaiDongGoi)
        {
            var result = await _donViTinhApiClient.GetDonViTinh(id, loaiDongGoi);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoDonViTinh request)
        {
            var result = await _donViTinhApiClient.Create(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _donViTinhApiClient.Delete(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatDonViTinh request)
        {
            var result = await _donViTinhApiClient.Update(request);
            return Ok(result);
        }
    }
}