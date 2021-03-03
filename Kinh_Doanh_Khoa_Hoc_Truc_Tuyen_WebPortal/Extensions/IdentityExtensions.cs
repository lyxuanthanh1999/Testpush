using System.Linq;
using System.Security.Claims;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Name);
            return claim?.Value;
        }

        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == "FullName");
            return claim?.Value;
        }

        public static string GetRole(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role);
            return claim?.Value;
        }

        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Email);
            return claim?.Value;
        }

        public static string GetAvatar(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == "Avatar");
            return claim?.Value;
        }

        public static string GetToken(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = ((ClaimsIdentity)claimsPrincipal.Identity)
                .Claims
                .FirstOrDefault(x => x.Type == "access_token");
            return claim?.Value;
        }
    }
}