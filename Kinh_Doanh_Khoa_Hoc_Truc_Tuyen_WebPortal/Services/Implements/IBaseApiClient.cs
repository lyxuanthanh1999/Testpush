using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements
{
    public interface IBaseApiClient
    {
        Task<List<T>> GetListAsync<T>(string url, bool requiredLogin = false);

        Task<T> GetAsync<T>(string url, bool requiredLogin = false);

        Task<bool> PutAsync<TRequest>(string url, TRequest requestContent, bool requiredLogin = true);

        Task<bool> Delete(string url, bool requiredLogin = true);

        Task<bool> PostReturnBooleanAsync<TRequest>(string url, TRequest requestContent, bool requiredLogin = true);

        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest requestContent, bool requiredLogin = true);

        Task<bool> PutForFileAsync<TResponse>(string url, MultipartFormDataContent requestContent, bool requiredLogin = true);

        Task<bool> PostForFileAsync<TResponse>(string url, MultipartFormDataContent requestContent, bool requiredLogin = true);

        Task<string> GetStringAsync(string url, bool requiredLogin = false);

        Task<string> PostReturnStringAsync<TRequest>(string url, TRequest requestContent, bool requiredLogin = true);

        Task<TResponse> PutHasResponseAsync<TRequest, TResponse>(string url, TRequest requestContent, bool requiredLogin = true);
    }
}