using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<LessonsController> _logger;

        public readonly UserManager<AppUser> _userManager;

        private readonly IStorageService _storageService;

        public LessonsController(IWebHostEnvironment webHostEnvironment, EKhoaHocDbContext khoaHocDbContext, ILogger<LessonsController> logger, IStorageService storageService, UserManager<AppUser> userManager)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _storageService = storageService;
            _userManager = userManager;
            FFmpeg.SetExecutablesPath(Path.Combine(webHostEnvironment.WebRootPath, "ffmpeg", "bin"));
        }

        [HttpGet("{id}")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.View)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                _logger.LogError($"Cannot found lesson with id {id}");
                return NotFound(new ApiNotFoundResponse($"Cannot found lesson with id {id}"));
            }
            return Ok(new LessonViewModel
            {
                Name = result.Name,
                Attachment = _storageService.GetFileUrl(result.Attachment),
                VideoPath = _storageService.GetFileUrl(result.VideoPath),
                Status = result.Status,
                SortOrder = result.SortOrder,
                CourseId = result.CourseId,
                Id = result.Id
            });
        }

        [HttpPost]
        [ValidationFilter]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Create)]
        public async Task<IActionResult> PostLesson([FromForm] LessonCreateRequest request)
        {
            var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == request.CourseId);
            if (course == null)
            {
                _logger.LogError($"Course with id {request.CourseId} is existed.");
                return BadRequest(new ApiBadRequestResponse($"Không tồn tại khóa học với id {request.CourseId}"));
            }
            if (request.VideoPath == null)
            {
                _logger.LogError($"Bạn phải thêm video cho bài học.");
                return BadRequest(new ApiBadRequestResponse($"Bạn phải thêm video cho bài học."));
            }
            var lesson = new Lesson
            {
                Name = request.Name,
                Status = request.Status,
                SortOrder = request.SortOrder,
                CourseId = request.CourseId,
            };
            if (request.VideoPath != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.VideoPath.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.VideoPath.OpenReadStream(), fileName, "videos");
                lesson.VideoPath = "videos/" + fileName;
                var pathToVideoFile = _storageService.GetFileRoot(fileName, "videos");
                var mediaInfo = await FFmpeg.GetMediaInfo(pathToVideoFile);
                lesson.Times = $@"{mediaInfo.VideoStreams.First().Duration:hh\:mm\:ss}";
            }
            if (request.Attachment != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Attachment.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Attachment.OpenReadStream(), fileName, "files");
                lesson.Attachment = "files/" + fileName;
            }
            await _khoaHocDbContext.Lessons.AddAsync(lesson);

            var result = await _khoaHocDbContext.SaveChangesAsync();

            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Create lesson is failed");
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpPut("approve")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> PutLessonsApprove(List<int> input)
        {
            var lstAnnouncement = new List<AnnouncementToUserViewModel>();
            foreach (var id in input)
            {
                var lessons = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
                if (lessons == null)
                {
                    _logger.LogError($"Cannot found lesson with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy bài học với id {id}"));
                }
                lessons.Status = 1;
                _khoaHocDbContext.Lessons.Update(lessons);
                var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == lessons.CourseId);
                var userCreated = await _userManager.FindByNameAsync(course.CreatedUserName);
                var userCurrent = await _userManager.FindByNameAsync(User.Identity.Name);
                var announceId = Guid.NewGuid();
                var announcement = new Announcement();
                announcement.UserId = userCurrent.Id;
                announcement.Status = 1;
                announcement.Content = $"{userCurrent.Name} đã duyệt bài {lessons.Name}";
                announcement.Title = "Bài học đã được duyệt";
                announcement.Id = announceId;
                announcement.EntityId = lessons.Id.ToString();
                announcement.EntityType = "lessons";
                announcement.Image = course.Image;
                await _khoaHocDbContext.Announcements.AddAsync(announcement);
                var announceUser = new AnnouncementUser()
                {
                    UserId = userCreated.Id,
                    AnnouncementId = announceId,
                    HasRead = false
                };
                await _khoaHocDbContext.AnnouncementUsers.AddAsync(announceUser);
                var result = await _khoaHocDbContext.SaveChangesAsync();
                var announcementViewmodel = new AnnouncementViewModel();
                announcementViewmodel.UserId = announcement.UserId;
                announcementViewmodel.Status = announcement.Status;
                announcementViewmodel.Content = announcement.Content;
                announcementViewmodel.Title = announcement.Title;
                announcementViewmodel.Id = announcement.Id;
                announcementViewmodel.EntityId = announcement.EntityId;
                announcementViewmodel.EntityType = announcement.EntityType;
                announcementViewmodel.Image = announcement.Image;
                announcementViewmodel.CreationTime = announcement.CreationTime;
                announcementViewmodel.LastModificationTime = announcement.LastModificationTime;
                lstAnnouncement.Add(new AnnouncementToUserViewModel()
                {
                    AnnouncementViewModel = announcementViewmodel,
                    UserId = userCreated.Id.ToString()
                });
                if (result < 0)
                {
                    _logger.LogError("Update lesson failed");
                    return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
                }
            }
            return Ok(lstAnnouncement);
        }

        [HttpGet("course-{id}")]
        public async Task<IActionResult> GetLessons(int id)
        {
            return Ok(await _khoaHocDbContext.Lessons.Include(x => x.Course).Where(x => x.CourseId == id && x.Status == 1).OrderBy(x => x.SortOrder).AsNoTracking().Select(_ => new LessonViewModel
            {
                Name = _.Name,
                Status = _.Status,
                SortOrder = _.SortOrder,
                CourseId = _.CourseId,
                Attachment = _storageService.GetFileUrl(_.Attachment),
                VideoPath = _storageService.GetFileUrl(_.VideoPath),
                Id = _.Id,
                CourseName = _.Course.Name,
                Times = _.Times
            }).ToListAsync());
        }

        [HttpGet("course-{id}/detail")]
        public IActionResult GetLessonsByCourseId(int id, int? lessonId)
        {
            var data = new LessonViewModel();
            if (lessonId == null)
            {
                var query = _khoaHocDbContext.Lessons.Include(x => x.Course)
                    .FirstOrDefault(x => x.CourseId == id && x.SortOrder == 1 && x.Status == 1);
                if (query == null) return Ok(data);
                data.Name = query.Name;
                data.Status = query.Status;
                data.SortOrder = query.SortOrder;
                data.CourseId = query.CourseId;
                data.Attachment = _storageService.GetFileUrl(query.Attachment);
                data.VideoPath = _storageService.GetFileUrl(query.VideoPath);
                data.Id = query.Id;
                data.CourseName = query.Course.Name;
                data.Times = query.Times;
            }
            else
            {
                var query = _khoaHocDbContext.Lessons.Include(x => x.Course)
                    .FirstOrDefault(x => x.CourseId == id && x.Id == lessonId && x.Status == 1);
                if (query == null) return Ok(data);
                data.Name = query.Name;
                data.Status = query.Status;
                data.SortOrder = query.SortOrder;
                data.CourseId = query.CourseId;
                data.Attachment = _storageService.GetFileUrl(query.Attachment);
                data.VideoPath = _storageService.GetFileUrl(query.VideoPath);
                data.Id = query.Id;
                data.CourseName = query.Course.Name;
                data.Times = query.Times;
            }

            return Ok(data);
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.View)]
        public IActionResult GetLessonsPaging(int courseId, string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Lessons.Include(x => x.Course).Where(x => x.CourseId == courseId).AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.Name.ToLower().Contains(filter.ToLower()) || x.Name.convertToUnSign().ToLower()
                        .Contains(filter.convertToUnSign().ToLower()));
            }

            var data = query.ToList();
            var totalRecords = data.Count();
            var items = data.OrderBy(x => x.SortOrder).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(_ => new LessonViewModel
            {
                Name = _.Name,
                Status = _.Status,
                Id = _.Id,
                SortOrder = _.SortOrder,
                CourseId = _.CourseId,
                Attachment = _storageService.GetFileUrl(_.Attachment),
                VideoPath = _storageService.GetFileUrl(_.VideoPath),
                CourseName = _.Course.Name
            }).ToList();
            var pagination = new Pagination<LessonViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpPut("{id}")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> PutLesson(int id, [FromForm] LessonCreateRequest request)
        {
            var lesson = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
            if (lesson == null)
            {
                _logger.LogError($"Cannot found lesson with id {id}");
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy bài học với id {id}"));
            }

            lesson.Name = request.Name;
            lesson.Status = request.Status;
            lesson.SortOrder = request.SortOrder;
            lesson.CourseId = request.CourseId;
            if (request.VideoPath != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.VideoPath.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.VideoPath.OpenReadStream(), fileName, "videos");
                lesson.VideoPath = "videos/" + fileName;
                var pathToVideoFile = _storageService.GetFileRoot(fileName, "videos");
                var mediaInfo = await FFmpeg.GetMediaInfo(pathToVideoFile);
                lesson.Times = $@"{mediaInfo.VideoStreams.First().Duration:hh\:mm\:ss}";
            }
            if (request.Attachment != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Attachment.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Attachment.OpenReadStream(), fileName, "files");
                lesson.Attachment = "files/" + fileName;
            }
            _khoaHocDbContext.Lessons.Update(lesson);
            var announcementToUser = new AnnouncementToUserViewModel();
            int result;
            if (request.Status == 1)
            {
                var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == lesson.CourseId);
                var userCreated = await _userManager.FindByNameAsync(course.CreatedUserName);
                var userCurrent = await _userManager.FindByNameAsync(User.Identity.Name);
                var announceId = Guid.NewGuid();
                var announcement = new Announcement();
                announcement.UserId = userCurrent.Id;
                announcement.Status = 1;
                announcement.Content = $"{userCurrent.Name} đã duyệt bài {lesson.Name}";
                announcement.Title = "Bài học đã được duyệt";
                announcement.Id = announceId;
                announcement.EntityId = lesson.Id.ToString();
                announcement.EntityType = "lessons";
                announcement.Image = course.Image;
                await _khoaHocDbContext.Announcements.AddAsync(announcement);
                var announceUser = new AnnouncementUser()
                {
                    UserId = userCreated.Id,
                    AnnouncementId = announceId,
                    HasRead = false
                };
                await _khoaHocDbContext.AnnouncementUsers.AddAsync(announceUser);
                result = await _khoaHocDbContext.SaveChangesAsync();
                var announcementViewmodel = new AnnouncementViewModel();
                announcementViewmodel.UserId = announcement.UserId;
                announcementViewmodel.Status = announcement.Status;
                announcementViewmodel.Content = announcement.Content;
                announcementViewmodel.Title = announcement.Title;
                announcementViewmodel.Id = announcement.Id;
                announcementViewmodel.EntityId = announcement.EntityId;
                announcementViewmodel.EntityType = announcement.EntityType;
                announcementViewmodel.Image = announcement.Image;
                announcementViewmodel.CreationTime = announcement.CreationTime;
                announcementViewmodel.LastModificationTime = announcement.LastModificationTime;
                announcementToUser.AnnouncementViewModel = announcementViewmodel;
                announcementToUser.UserId = userCreated.Id.ToString();
            }
            else
            {
                result = await _khoaHocDbContext.SaveChangesAsync();
            }
            if (result > 0)
            {
                return Ok(announcementToUser);
            }
            _logger.LogError("Update lesson failed");
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPost("{id}")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Delete)]
        public async Task<IActionResult> DeleteLesson(List<int> input)
        {
            foreach (var id in input)
            {
                var lesson = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id == id);
                if (lesson == null)
                {
                    _logger.LogError($"Cannot found lesson with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy bài học với id {id}"));
                }
                var comments = _khoaHocDbContext.Comments.Where(x => x.EntityId == lesson.Id && x.EntityType.Equals("Lessons"));
                if (comments.Any())
                {
                    _khoaHocDbContext.Comments.RemoveRange(comments);
                }
                _khoaHocDbContext.Lessons.Remove(lesson);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Delete lesson failed");
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }

        [HttpDelete("course-{coursesId}-lesson-{lessonId}/attachment")]
        public async Task<IActionResult> DeleteAttachment(int coursesId, int lessonId)
        {
            var lesson = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id.Equals(lessonId) && x.CourseId == coursesId);
            if (lesson == null)
                return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy file với id {lessonId}"));
            lesson.Attachment = null;
            _khoaHocDbContext.Lessons.Update(lesson);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }

        [HttpDelete("course-{coursesId}-lesson-{lessonId}/video")]
        public async Task<IActionResult> DeleteVideo(int coursesId, int lessonId)
        {
            var lesson = await _khoaHocDbContext.Lessons.FirstOrDefaultAsync(x => x.Id.Equals(lessonId) && x.CourseId == coursesId);
            if (lesson == null)
                return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy bài học với id {lessonId}"));
            lesson.VideoPath = "";
            _khoaHocDbContext.Lessons.Update(lesson);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }
    }
}