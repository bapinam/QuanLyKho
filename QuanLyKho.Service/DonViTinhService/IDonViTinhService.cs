using QuanLyKho.Data.Extensions.Enums;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.DonViTinh;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.DonViTinhService
{
    public interface IDonViTinhService
    {
        Task<ApiResult<DonViTinhModel>> Create(TaoDonViTinh bundle);

        Task<ApiResult<bool>> Update(CapNhatDonViTinh bundle);

        Task<ApiResult<bool>> Delete(long id);

        Task<List<DonViTinhModel>> GetDonViTinh(int id, LoaiDongGoi loaiDongGoi);

        Task<ApiResult<bool>> iTen(string ten, int id, long? idDVT, bool loaiDongGoi);
    }
}