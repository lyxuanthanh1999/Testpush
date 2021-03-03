using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.HubConfig;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<CategoriesController> _logger;

        public readonly IStorageService _storageService;

        public readonly UserManager<AppUser> _userManager;

        public readonly IHubContext<ChatHub> _hubContext;

        public AnnouncementsController(EKhoaHocDbContext khoaHocDbContext, ILogger<CategoriesController> logger, IStorageService storageService, UserManager<AppUser> userManager, IHubContext<ChatHub> hubContext)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _storageService = storageService;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        [HttpGet("private-paging/filter")]
        public async Task<IActionResult> GetAllUnReadPaging(string filter, string userId, int pageIndex, int pageSize)
        {
            IQueryable<Announcement> query;
            if (filter.Equals("true"))
            {
                query = from x in _khoaHocDbContext.Announcements.AsNoTracking()
                        join y in _khoaHocDbContext.AnnouncementUsers.AsNoTracking()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from announceUser in xy.DefaultIfEmpty()
                        where x.Status == 1 && announceUser.UserId == Guid.Parse(userId)
                        orderby !announceUser.HasRead descending, x.CreationTime descending
                        select x;
            }
            else
            {
                query = from x in _khoaHocDbContext.Announcements.AsNoTracking()
                        join y in _khoaHocDbContext.AnnouncementUsers.AsNoTracking()
                            on x.Id equals y.AnnouncementId
                            into xy
                        from announceUser in xy.DefaultIfEmpty()
                        where announceUser.HasRead == false && x.Status == 1 && announceUser.UserId == Guid.Parse(userId)
                        orderby !announceUser.HasRead descending, x.CreationTime descending
                        select x;
            }
            var lstAnnounce = new List<AnnouncementViewModel>();
            var items = query.OrderByDescending(x => x.CreationTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            foreach (var announcement in items.ToList())
            {
                var announceViewModel = new AnnouncementViewModel();
                announceViewModel.UserId = announcement.UserId;
                announceViewModel.Status = announcement.Status;
                announceViewModel.EntityType = announcement.EntityType;
                announceViewModel.CreationTime = announcement.CreationTime;
                announceViewModel.LastModificationTime = announcement.LastModificationTime;
                announceViewModel.Content = announcement.Content;
                announceViewModel.EntityId = announcement.EntityId;
                announceViewModel.Title = announcement.Title;
                announceViewModel.Image = _storageService.GetFileUrl(announcement.Image);
                announceViewModel.Id = announcement.Id;
                if (announcement.UserId.HasValue)
                {
                    var user = await _userManager.FindByIdAsync(announcement.UserId.ToString());
                    announceViewModel.UserFullName = user.Name;
                }
                var announceUser = _khoaHocDbContext.AnnouncementUsers
                    .FirstOrDefault(x => x.AnnouncementId == announcement.Id && x.UserId == Guid.Parse(userId));
                if (announceUser != null)
                {
                    announceViewModel.TmpHasRead = announceUser.HasRead;
                }
                else
                {
                    announceViewModel.TmpHasRead = true;
                }
                lstAnnounce.Add(announceViewModel);
            }
            var totalRow = query.Count();
            var model = lstAnnounce;
            return Ok(new Pagination<AnnouncementViewModel>
            {
                Items = model,
                TotalRecords = totalRow,
                PageSize = pageSize,
                PageIndex = pageIndex
            });
        }

        [HttpPut("mark-read")]
        public async Task<IActionResult> MarkAsRead(AnnouncementMarkReadRequest request)
        {
            var result = false;
            var announce = await _khoaHocDbContext.AnnouncementUsers.AsNoTracking().FirstOrDefaultAsync(_ =>
                _.AnnouncementId == Guid.Parse(request.AnnounceId) && _.UserId == Guid.Parse(request.UserId));
            if (announce != null)
            {
                if (announce.HasRead == false)
                {
                    announce.HasRead = true;
                    _khoaHocDbContext.AnnouncementUsers.Update(announce);
                    await _khoaHocDbContext.SaveChangesAsync();
                    result = true;
                }
            }

            return Ok(result);
        }

        [HttpPost("create-announce")]
        [ValidationFilter]
        public async Task<IActionResult> PostAnnouncement([FromBody] AnnouncementCreateRequest request)
        {
            if (request == null)
            {
                return BadRequest(new ApiBadRequestResponse("Lỗi thông báo"));
            }
            try
            {
                var announceId = Guid.NewGuid();
                var announcement = new Announcement();
                announcement.Status = request.Status;
                announcement.Content = request.Content;
                announcement.EntityId = request.EntityId;
                announcement.EntityType = request.EntityType;
                announcement.Id = announceId;
                announcement.Image = request.Image ?? "images/defaultAvatar.png";
                announcement.Title = request.Title;
                announcement.UserId = Guid.Parse(request.UserId);
                await _khoaHocDbContext.Announcements.AddAsync(announcement);
                var users = _userManager.Users.AsNoTracking();
                foreach (var user in users)
                {
                    var announceUser = new AnnouncementUser();
                    announceUser.HasRead = false;
                    announceUser.UserId = user.Id;
                    announceUser.AnnouncementId = announceId;
                    await _khoaHocDbContext.AddAsync(announceUser);
                }
                await _khoaHocDbContext.SaveChangesAsync();
                var announceVm = new AnnouncementViewModel();
                announceVm.Status = request.Status;
                announceVm.Content = request.Content;
                announceVm.EntityId = request.EntityId;
                announceVm.EntityType = request.EntityType;
                announceVm.Id = announcement.Id;
                announceVm.Image = _storageService.GetFileUrl(announcement.Image);
                announceVm.Title = request.Title;
                return Ok(announceVm);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiBadRequestResponse(e.Message));
            }
        }

        [HttpGet("send-announce-{id}/server")]
        [ValidationFilter]
        public async Task<IActionResult> SendAnnouncementByServer(string id)
        {
            var announce = _khoaHocDbContext.Announcements.AsNoTracking().FirstOrDefault(x => x.Id == Guid.Parse(id));
            if (announce != null)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", announce);
                return Ok();
            }

            return BadRequest(new ApiBadRequestResponse("Không tìm thấy thông báo"));
        }

        [HttpGet("{announceId}")]
        public async Task<IActionResult> GetDetailAnnouncement(string announceId, string receiveId)
        {
            var data = await (from x in _khoaHocDbContext.Announcements.AsNoTracking()
                              join y in _khoaHocDbContext.AnnouncementUsers.AsNoTracking()
                                  on x.Id equals y.AnnouncementId
                                  into xy
                              from announceUser in xy.DefaultIfEmpty()
                              where announceUser.UserId == Guid.Parse(receiveId) && x.Id == Guid.Parse(announceId)
                              orderby !announceUser.HasRead descending, x.CreationTime descending
                              select x).FirstOrDefaultAsync();
            var announceViewModel = new AnnouncementViewModel();
            announceViewModel.UserId = data.UserId;
            announceViewModel.Status = data.Status;
            announceViewModel.EntityType = data.EntityType;
            announceViewModel.CreationTime = data.CreationTime;
            announceViewModel.LastModificationTime = data.LastModificationTime;
            announceViewModel.Content = data.Content;
            announceViewModel.EntityId = data.EntityId;
            announceViewModel.Title = data.Title;
            announceViewModel.Image = _storageService.GetFileUrl(data.Image);
            announceViewModel.Id = data.Id;
            if (data.UserId.HasValue)
            {
                var user = await _userManager.FindByIdAsync(data.UserId.ToString());
                announceViewModel.UserFullName = user.Name;
            }
            var announcementUser = _khoaHocDbContext.AnnouncementUsers
                .FirstOrDefault(x => x.AnnouncementId == data.Id);
            if (announcementUser != null)
            {
                announceViewModel.TmpHasRead = announcementUser.HasRead;
            }
            else
            {
                announceViewModel.TmpHasRead = true;
            }
            return Ok(announceViewModel);
        }
    }
}