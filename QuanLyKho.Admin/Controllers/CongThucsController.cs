using Microsoft.AspNetCore.Mvc;
using QuanLyKho.ApiIntegration.CongThucApiClient;
using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.CongThuc;
using QuanLyKho.WebAppAdmin.Controllers.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyKho.Admin.Controllers
{
    public class CongThucsController : BaseController
    {
        private readonly ICongThucApiClient _congThucApiClient;

        public CongThucsController(ICongThucApiClient congThucApiClient)
        {
            _congThucApiClient = congThucApiClient;
        }

        public async Task<IActionResult> Index(string tuKhoa, int idLoaiSanPham, int pageIndex = 1, int pageSize = 10)
        {
            var request = new GetCongThucSPPaging()
            {
                TuKhoa = tuKhoa,
                IdLoaiSanPham = idLoaiSanPham,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _congThucApiClient.GetCongThucSPPaging(request);

            ViewBag.Keyword = tuKhoa;

            return View(data.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> GetListCongThuc(int id)
        {
            var result = await _congThucApiClient.GetListCongThuc(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> iTen(string ten, int idSP, int? id)
        {
            var result = await _congThucApiClient.iTen(ten, idSP, id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> iNguyenVatLieu(int idCongThuc, int idNguyenVatLieu)
        {
            var result = await _congThucApiClient.iNguyenVatLieu(idCongThuc, idNguyenVatLieu);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiNguyenVatLieu(NhomLoaiNVL? group)
        {
            var result = await _congThucApiClient.GetLoaiNguyenVatLieu(group);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListNguyenVatLieu(ListBundleNguyenVatLieu bundle)
        {
            var result = await _congThucApiClient.GetListNguyenVatLieu(bundle);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListDonViTinhs(int id)
        {
            var result = await _congThucApiClient.GetListDonViTinhs(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListIdCongThuc(int id)
        {
            var result = await _congThucApiClient.GetListIdCongThuc(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaoCongThuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucApiClient.Create(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDinhDauCongThuc(CapNhatDinhDauCongThuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucApiClient.UpdateDinhDauCongThuc(bundle);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CapNhatCongThuc bundle)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucApiClient.Update(bundle);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id, int idSanPham)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucApiClient.Delete(id, idSanPham);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNguyenVatLieu(long id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _congThucApiClient.DeleteNguyenVatLieu(id);
            return Ok(result);
        }
    }
}