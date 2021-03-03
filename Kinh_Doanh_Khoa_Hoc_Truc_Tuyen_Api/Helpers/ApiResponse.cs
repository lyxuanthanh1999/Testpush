using Newtonsoft.Json;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers
{
    public class ApiResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return "Không tìm thấy trang này";

                case 500:
                    return "Lỗi server";

                default:
                    return null;
            }
        }
    }
}