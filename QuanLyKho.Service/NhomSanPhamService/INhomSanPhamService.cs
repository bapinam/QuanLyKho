﻿using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.NhomSanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.NhomSanPhamService
{
    public interface INhomSanPhamService
    {
        Task<ApiResult<NhomSanPhamModel>> Create(TaoNhomSanPham bundle);

        Task<ApiResult<bool>> Update(int id, CapNhatNhomSanPham bundle);

        Task<ApiResult<PagedResult<NhomSanPhamModel>>> GetNhomSanPhamPaging(GetNhomSanPham bundle);

        Task<ApiResult<NhomSanPhamModel>> GetById(int id);

        Task<ApiResult<bool>> Delete(int id);

        Task<ApiResult<bool>> iTen(string ten, int? id);

        Task<List<GetTenNhomSP>> GetTenNhomSP();
    }
}