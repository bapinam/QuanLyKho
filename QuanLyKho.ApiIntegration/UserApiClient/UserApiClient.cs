using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuanLyKho.ApiIntegration.Common;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using QuanLyKho.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.UserApiClient
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration) : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            var response = await client.PostAsync("/api/user/authenticate", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest bundle)
        {
            var url = $"/api/User/paging?pageIndex=" +
                            $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}";
            var result = await GetAsync<ApiResult<PagedResult<UserVm>>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var url = $"/api/user/" + $"{id}";
            var result = await Delete<bool>(url);
            return result;
        }

        public async Task<ApiResult<GetByIdListUser>> GetById(Guid id)
        {
            var url = $"/api/user/" + $"{id}";
            var result = await GetAsync<ApiResult<GetByIdListUser>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iCard(string card, Guid? id)
        {
            var url = $"/api/user/check-card?card=" + $"{card}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iEmail(string email, Guid? id)
        {
            var url = $"/api/user/check-email?email=" + $"{email}&id={id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<UserNameVm>> RegisterUser(RegisterRequest registerRequest)
        {
            var url = $"/api/user";
            var result = await Create<UserNameVm, RegisterRequest>(url, registerRequest);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            var url = $"/api/user/" + $"{id}";
            var result = await Update<bool, UserUpdateRequest>(url, request);
            return result;
        }

        public async Task<ApiResult<string>> UpdateJobStauts(UpdateJobStauts bundle)
        {
            var url = $"/api/user/job-stautus";
            var result = await Update<string, UpdateJobStauts>(url, bundle);
            return result;
        }

        public async Task<ApiResult<GetByIdListUser>> GetByName(string name)
        {
            var url = $"/api/user/name/" + $"{name}";
            var result = await GetAsync<ApiResult<GetByIdListUser>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> iEmailName(string email, string name)
        {
            var url = $"/api/user/check-email-name?email=" + $"{email}&name={name}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> UpdateInfor(UpdateInfor bundle)
        {
            var url = $"/api/user/infor";
            var result = await Update<bool, UpdateInfor>(url, bundle);
            return result;
        }

        public async Task<ApiResult<bool>> UpdatePassword(UserUpdatePassword bundle)
        {
            var url = $"/api/user/change-password";
            var result = await Update<bool, UserUpdatePassword>(url, bundle);
            return result;
        }

        public async Task<ApiResult<string>> UpdateImage(UpdateImageUser bundle)
        {
            var requestContent = new MultipartFormDataContent();

            byte[] data;
            using (var br = new BinaryReader(bundle.File.OpenReadStream()))
            {
                data = br.ReadBytes((int)bundle.File.OpenReadStream().Length);
            }
            ByteArrayContent bytes = new ByteArrayContent(data);
            requestContent.Add(bytes, "file", bundle.File.FileName);

            requestContent.Add(new StringContent(bundle.Ten.ToString()), "ten");

            var sessions = _httpContextAccessor
              .HttpContext
              .Session
              .GetString(SystemConstants.AppSettings.Token);
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(SystemConstants.Bearer, sessions);

            var url = $"/api/User/image/";
            var response = await client.PutAsync(url, requestContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(result);
        }

        public async Task<ApiResult<string>> GetImage(string name)
        {
            var url = $"/api/user/get-image/" + $"{name}";
            var result = await GetAsync<ApiResult<string>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> ResetPassWord(Guid id)
        {
            var url = $"/api/user/reset-password/" + $"{id}";
            var result = await GetAsync<ApiResult<bool>>(url);
            return result;
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUserRole(GetUserPagingRequest bundle)
        {
            var url = $"/api/User/paging-role?pageIndex=" +
                         $"{bundle.PageIndex}&pageSize={bundle.PageSize}&tuKhoa={bundle.TuKhoa}";
            var result = await GetAsync<ApiResult<PagedResult<UserVm>>>(url);
            return result;
        }

        public async Task<List<VaiTroVM>> GetRole(Guid id)
        {
            var url = $"/api/user/get-role/" + $"{id}";
            var result = await GetAsync<List<VaiTroVM>>(url);
            return result;
        }

        public async Task<ApiResult<bool>> GiaoQuyenHan(GiaoQuyenHanNguoiDung bundle)
        {
            var url = $"/api/user/giao-quyen";
            var result = await Update<bool, GiaoQuyenHanNguoiDung>(url, bundle);
            return result;
        }
    }
}