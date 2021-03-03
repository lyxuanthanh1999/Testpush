using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers
{
    public class ContactController : Controller
    {
        private readonly IBaseApiClient _apiClient;

        private readonly IConfiguration _configuration;

        private readonly IEmailSender _emailSender;

        private readonly IViewRenderService _viewRenderService;

        public ContactController(IBaseApiClient apiClient, IConfiguration configuration, IEmailSender emailSender, IViewRenderService viewRenderService)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _emailSender = emailSender;
            _viewRenderService = viewRenderService;
        }

        [HttpGet("contact.html")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("contact.html")]
        public async Task<IActionResult> Index(FeedBackCreateRequest feedBackRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(feedBackRequest);
                }
                var result = await _apiClient.PostAsync<FeedBackCreateRequest, FeedBackViewModel>("/api/feedBacks", feedBackRequest, false);
                var content = await _viewRenderService.RenderToStringAsync("Contact/ContactMail", result);
                await _emailSender.SendEmailAsync(_configuration["MailSettings:AdminMail"], "Bạn Có 1 Phản Hồi Mới", content);
                return RedirectToAction(nameof(FeedBackConfirmation));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        [HttpGet("about.html")]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult FeedBackConfirmation()
        {
            return View();
        }
    }
}