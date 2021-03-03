using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailSender(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient(_configuration["MailSettings:Server"])
            {
                UseDefaultCredentials = false,
                Port = int.Parse(_configuration["MailSettings:Port"]),
                EnableSsl = bool.Parse(_configuration["MailSettings:EnableSsl"]),
                Credentials = new NetworkCredential(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["MailSettings:FromEmail"], _configuration["MailSettings:FromName"]),
            };
            mailMessage.To.Add(email);
            mailMessage.Body = message;
            mailMessage.Subject = subject;
            mailMessage.IsBodyHtml = true;
            var session = _httpContextAccessor.HttpContext.Session.Get<ResultFileResponse>(SystemConstants.AttachmentSession);
            if (session != null)
            {
                if (!string.IsNullOrEmpty(session.FileExcel))
                {
                    var fileNameExcel = Path.GetFileName(session.FileExcel);
                    var myClient = new WebClient();
                    var bytes = myClient.DownloadData(session.FileExcel);
                    var webExcel = new MemoryStream(bytes);
                    mailMessage.Attachments.Add(new Attachment(webExcel, fileNameExcel));
                }
                if (!string.IsNullOrEmpty(session.FilePdf))
                {
                    var fileNamePdf = Path.GetFileName(session.FilePdf);
                    var myClient = new WebClient();
                    var bytes = myClient.DownloadData(session.FilePdf);
                    var webPdf = new MemoryStream(bytes);
                    mailMessage.Attachments.Add(new Attachment(webPdf, fileNamePdf));
                }
            }
            client.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}