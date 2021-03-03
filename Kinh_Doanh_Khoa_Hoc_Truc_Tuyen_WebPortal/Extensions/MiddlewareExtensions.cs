using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}