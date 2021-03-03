using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services
{
    public class BaseApiClient : IBaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseApiClient(IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var data = (List<T>)JsonConvert.DeserializeObject(body, typeof(List<T>));
            return data;
        }

        public async Task<T> GetAsync<T>(string url, bool requiredLogin = false)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(body);
            return data;
        }

        public async Task<string> GetStringAsync(string url, bool requiredLogin = false)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();
            var data = body;
            return data;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            StringContent httpContent = null;
            if (requestContent != null)
            {
                var json = JsonConvert.SerializeObject(requestContent);
                httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            }

            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PostAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResponse>(body);
            }
            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<string> PostReturnStringAsync<TRequest>(string url, TRequest requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            StringContent httpContent = null;
            if (requestContent != null)
            {
                var json = JsonConvert.SerializeObject(requestContent);
                httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            }

            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PostAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<string>(body);
            }
            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<bool> PostReturnBooleanAsync<TRequest>(string url, TRequest requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            StringContent httpContent = null;
            if (requestContent != null)
            {
                var json = JsonConvert.SerializeObject(requestContent);
                httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            }

            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PostAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<bool> PostForFileAsync<TResponse>(string url, MultipartFormDataContent requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PostAsync(url, requestContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<bool> PutForFileAsync<TResponse>(string url, MultipartFormDataContent requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PutAsync(url, requestContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<bool> PutAsync<TRequest>(string url, TRequest requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            HttpContent httpContent = null;
            if (requestContent != null)
            {
                var json = JsonConvert.SerializeObject(requestContent);
                httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            }

            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PutAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;

            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<TResponse> PutHasResponseAsync<TRequest, TResponse>(string url, TRequest requestContent, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            HttpContent httpContent = null;
            if (requestContent != null)
            {
                var json = JsonConvert.SerializeObject(requestContent);
                httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            }

            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.PutAsync(url, httpContent);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<TResponse>(body);

            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }

        public async Task<bool> Delete(string url, bool requiredLogin = true)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration[SystemConstants.BaseAddress]);
            if (requiredLogin)
            {
                var token = _httpContextAccessor.HttpContext.User.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await client.DeleteAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return true;
            throw new Exception(JsonConvert.DeserializeObject<ApiResponse>(body).Message);
        }
    }
}