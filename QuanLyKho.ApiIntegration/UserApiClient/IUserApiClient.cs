using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.UserApiClient
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest bundle);

        Task<ApiResult<UserNameVm>> RegisterUser(RegisterRequest registerRequest);

        Task<ApiResult<bool>> iCard(string card, Guid? id);

        Task<ApiResult<bool>> iEmail(string email, Guid? id);

        Task<ApiResult<bool>> iEmailName(string email, string name);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ApiResult<bool>> UpdateInfor(UpdateInfor bundle);

        Task<ApiResult<GetByIdListUser>> GetById(Guid id);

        Task<ApiResult<GetByIdListUser>> GetByName(string name);

        Task<ApiResult<string>> GetImage(string name);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<string>> UpdateJobStauts(UpdateJobStauts bundle);

        Task<ApiResult<bool>> UpdatePassword(UserUpdatePassword bundle);

        Task<ApiResult<string>> UpdateImage(UpdateImageUser bundle);

        Task<ApiResult<bool>> ResetPassWord(Guid id);

        Task<ApiResult<PagedResult<UserVm>>> GetUserRole(GetUserPagingRequest bundle);

        Task<List<VaiTroVM>> GetRole(Guid id);
        Task<ApiResult<bool>> GiaoQuyenHan(GiaoQuyenHanNguoiDung bundle);

    }
}