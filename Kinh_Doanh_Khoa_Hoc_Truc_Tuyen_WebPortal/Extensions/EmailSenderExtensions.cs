using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Xác nhận email",
                $"Bạn hãy nhấn vào đây để xác nhận email: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}