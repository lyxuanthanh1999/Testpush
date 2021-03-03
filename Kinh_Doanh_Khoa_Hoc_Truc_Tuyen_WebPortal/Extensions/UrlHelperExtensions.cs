using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, Guid userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, Guid userId, string token, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, token },
                protocol: scheme);
        }
    }
}