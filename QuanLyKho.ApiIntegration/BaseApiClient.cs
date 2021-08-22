using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using QuanLyKho.Utilities.Constants;
using QuanLyKho.ViewModels.Common;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyKho.ApiIntegration.Common
{
    public class BaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient Client()
        {
            var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.Token);

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.AppSettings.BaseAddress]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(SystemConstants.Bearer, sessions);
            return client;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            var client = this.Client();
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<T>(body);

                return result;
            }

            throw new Exception(body);
        }

        protected async Task<ApiResult<T>> Delete<T>(string url)
        {
            var client = this.Client();

            var response = await client.DeleteAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<T>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(result);
        }

        protected async Task<ApiResult<T>> Update<T, B>(string url, B bundle)
        {
            var json = JsonConvert.SerializeObject(bundle);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = this.Client();

            var response = await client.PutAsync(url, httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<T>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(result);
        }

        public async Task<ApiResult<T>> UpdateHinhAnh<T>(string url, MultipartFormDataContent content)
        {
            var client = this.Client();

            var response = await client.PutAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<T>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(result);
        }

        protected async Task<ApiResult<T>> Create<T, B>(string url, B bundle)
        {
            var json = JsonConvert.SerializeObject(bundle);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = this.Client();

            var response = await client.PostAsync(url, httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ApiSuccessResult<T>>(result);

            return JsonConvert.DeserializeObject<ApiErrorResult<T>>(result);
        }

        protected async Task<T> iCheck<T>(string url)
        {
            var client = this.Client();

            var response = await client.GetAsync(url);

            var body = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<T>(body);

            return result;
        }
    }
}