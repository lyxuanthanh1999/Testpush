using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBacksController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<FeedBacksController> _logger;

        private readonly IConfiguration _configuration;

        public FeedBacksController(EKhoaHocDbContext khoaHocDbContext, ILogger<FeedBacksController> logger, IConfiguration configuration)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> PostFeedBack(FeedBackCreateRequest request)
        {
            var feedback = new FeedBack()
            {
                Message = request.Message,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };
            await _khoaHocDbContext.FeedBacks.AddAsync(feedback);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(new FeedBackViewModel
                {
                    Message = feedback.Message,
                    Name = feedback.Name,
                    LastModificationTime = feedback.LastModificationTime,
                    CreationTime = feedback.CreationTime,
                    Email = feedback.Email,
                    PhoneNumber = feedback.PhoneNumber,
                    Id = feedback.Id
                });
            }
            _logger.LogError("Create feedback failed");
            return BadRequest(new ApiBadRequestResponse("Gửi thất bại"));
        }
    }
}