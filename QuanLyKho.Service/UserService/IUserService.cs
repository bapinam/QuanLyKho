using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.Service.UserService
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest bundle);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest bundle);

        Task<ApiResult<Guid>> Register(RegisterRequest bundle);

        Task<ApiResult<bool>> iCard(string card, Guid? id);

        Task<ApiResult<bool>> iEmail(string email, Guid? id);

        Task<ApiResult<bool>> iEmailName(string email, string name);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest bundle);

        Task<ApiResult<bool>> UpdateInfor(UpdateInfor bundle);

        Task<ApiResult<bool>> UpdatePassword(UserUpdatePassword bundle);

        Task<ApiResult<GetByIdListUser>> GetById(Guid id);

        Task<ApiResult<GetByIdListUser>> GetByName(string name);

        Task<ApiResult<UserNameVm>> GetByUserName(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<string>> UpdateJobStauts(UpdateJobStauts bundle);

        Task<ApiResult<string>> UpdateImage(UpdateImageUser bundle);

        Task<ApiResult<string>> GetImage(string name);

        Task<ApiResult<bool>> ResetPassWord(Guid id);

        Task<ApiResult<PagedResult<UserVm>>> GetUserRole(GetUserPagingRequest bundle);

        Task<List<VaiTroVM>> GetRole(Guid id);

        Task<ApiResult<bool>> GiaoQuyenHan(GiaoQuyenHanNguoiDung bundle);
    }
}