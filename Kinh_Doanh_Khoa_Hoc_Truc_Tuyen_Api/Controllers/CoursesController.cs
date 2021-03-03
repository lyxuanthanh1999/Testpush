using Dapper;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<CoursesController> _logger;

        public readonly UserManager<AppUser> _userManager;

        private readonly IStorageService _storageService;

        private readonly IConfiguration _configuration;

        public CoursesController(EKhoaHocDbContext khoaHocDbContext, ILogger<CoursesController> logger, UserManager<AppUser> userManager, IStorageService storageService, IConfiguration configuration)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _userManager = userManager;
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@id", id);
            var result = (await conn.QueryAsync<CourseViewModel>("CourseDetail", dynamicParameters, null, 120, CommandType.StoredProcedure)).FirstOrDefault();
            if (result == null)
            {
                _logger.LogError($"Cannot found Course with id {id}");
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id {id}"));
            }

            var countStudent = _khoaHocDbContext.ActivateCourses.Include(x => x.AppUser).Count(x => x.CourseId == id && x.AppUser.UserName != result.CreatedUserName && x.Status);
            return Ok(new CourseViewModel
            {
                Name = result.Name,
                Image = _storageService.GetFileUrl(result.Image),
                CategoryId = result.CategoryId,
                Status = result.Status,
                Price = result.Price,
                Description = result.Description,
                Content = result.Content,
                CreationTime = result.CreationTime,
                LastModificationTime = result.LastModificationTime,
                CategoryName = result.CategoryName,
                Id = result.Id,
                DiscountAmount = result.DiscountAmount,
                DiscountPercent = result.DiscountPercent,
                CreatedUserName = result.CreatedUserName,
                CreatedName = result.CreatedName,
                CountStudent = countStudent
            });
        }

        [HttpPost]
        [ValidationFilter]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Create)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostCourse([FromForm] CourseCreateRequest request)
        {
            var dbCourse = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (dbCourse != null)
            {
                _logger.LogError($"Course with id {request.Id} is existed.");
                return BadRequest(new ApiBadRequestResponse($"Đã tồn tại khóa học này với id: {request.Id}"));
            }
            if (request.Image == null)
            {
                _logger.LogError($"Bạn phải thêm hình ảnh cho khóa học.");
                return BadRequest(new ApiBadRequestResponse($"Bạn phải thêm hình ảnh cho khóa học."));
            }
            var course = new Course
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                Status = request.Status,
                Description = request.Description,
                Content = request.Content,
                Price = request.Price,
                CreatedUserName = User.GetUserName()
            };
            if (request.Image != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Image.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Image.OpenReadStream(), fileName, "images");
                course.Image = "images/" + fileName;
            }
            await _khoaHocDbContext.Courses.AddAsync(course);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Create course is failed");
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpGet("new-courses")]
        public async Task<IActionResult> GetNewCourses()
        {
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var courses = (await conn.QueryAsync<CourseViewModel>("NewCourses", null, null, 120, CommandType.StoredProcedure)).ToList();
            return Ok(courses.Select(_ => new CourseViewModel
            {
                Id = _.Id,
                Name = _.Name.formatData(30),
                Image = _storageService.GetFileUrl(_.Image),
                CategoryId = _.CategoryId,
                Status = _.Status,
                Description = _.Description,
                Content = _.Content,
                Price = _.Price,
                CategoryName = _.CategoryName,
                DiscountAmount = _.DiscountAmount,
                DiscountPercent = _.DiscountPercent,
                CreatedUserName = _.CreatedUserName
            }).ToList());
        }

        [HttpGet("filter-name")]
        public async Task<IActionResult> GetFilterName()
        {
            var data = await _khoaHocDbContext.Courses.Select(x => x.Name).ToListAsync();
            return Ok(data);
        }

        [HttpGet("related-courses/{id}")]
        public async Task<IActionResult> GetRelatedCourses(int id)
        {
            var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id {id}"));
            }
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var parameters = new DynamicParameters();
            parameters.Add("@categoryId", course.CategoryId);
            parameters.Add("@courseId", course.Id);
            var courses = (await conn.QueryAsync<CourseViewModel>("RelatedCourses", parameters, null, 120, CommandType.StoredProcedure)).ToList();
            return Ok(courses.Select(_ => new CourseViewModel
            {
                Id = _.Id,
                Name = _.Name.formatData(30),
                Image = _storageService.GetFileUrl(_.Image),
                CategoryId = _.CategoryId,
                Status = _.Status,
                Description = _.Description,
                Content = _.Content,
                Price = _.Price,
                CategoryName = _.CategoryName,
                DiscountAmount = _.DiscountAmount,
                DiscountPercent = _.DiscountPercent,
                CreatedUserName = _.CreatedUserName
            }).ToList());
        }

        [HttpGet("filter")]
        public IActionResult GetCoursesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Courses.Include(x => x.Category).AsNoTracking().AsEnumerable();

            if (User != null && User.IsInRole("Teacher"))
            {
                query = query.Where(x => x.CreatedUserName == User.GetUserName());
            }
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.Name.ToLower().Contains(filter.ToLower()) || x.Name.convertToUnSign().ToLower()
                        .Contains(filter.convertToUnSign().ToLower()));
            }
            var data = query.ToList();
            var totalRecords = data.Count();
            var items = data.OrderByDescending(x => x.Status).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(_ => new CourseViewModel
            {
                Id = _.Id,
                Name = _.Name,
                CategoryId = _.CategoryId,
                Status = _.Status,
                Image = _storageService.GetFileUrl(_.Image),
                Description = _.Description,
                Content = _.Content,
                Price = _.Price,
                CategoryName = _.Category.Name
            }).ToList();
            var pagination = new Pagination<CourseViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpGet("client-filter")]
        public async Task<IActionResult> GetCoursesPagingForClient(int? categoryId, long? priceMin, long? priceMax, string sortBy, string filter, int pageIndex, int pageSize)
        {
            List<CourseViewModel> data;
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            if (categoryId != null)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@categoryId", categoryId);
                var category = await _khoaHocDbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
                data = category.ParentId == null
                    ? (await conn.QueryAsync<CourseViewModel>("ListCoursesByCategoryParentId", parameters, null, 120,
                        CommandType.StoredProcedure)).ToList()
                    : (await conn.QueryAsync<CourseViewModel>("ListCoursesByCategoryId", parameters, null, 120,
                        CommandType.StoredProcedure)).ToList();
            }
            else
            {
                data = (await conn.QueryAsync<CourseViewModel>("ListCourses", null, null, 120,
                    CommandType.StoredProcedure)).ToList();
            }
            var query = data.AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.ToLower()) ||
                                         x.Name.convertToUnSign().ToLower().Contains(filter.convertToUnSign().ToLower()));
            }
            if (priceMin != null && priceMax != null)
            {
                query = query.Where(x => x.SortPrice >= priceMin && x.SortPrice <= priceMax);
            }
            query = sortBy switch
            {
                "name" => query.OrderByDescending(x => x.Name),
                "price_low_to_high" => query.OrderBy(x => x.SortPrice),
                "price_high_to_low" => query.OrderByDescending(x => x.SortPrice),
                _ => query.OrderByDescending(x => x.CreationTime)
            };
            data = query.ToList();
            foreach (var item in data)
            {
                if (item.DiscountPercent > 0)
                {
                    item.SortPrice = item.Price - (item.Price * item.DiscountPercent / 100);
                }
                else if (item.DiscountAmount > 0)
                {
                    item.SortPrice = item.DiscountAmount;
                }
                else
                {
                    item.SortPrice = item.Price;
                }
            }
            var totalRecords = data.Count();
            var courses = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(c => new CourseViewModel()
            {
                Name = c.Name.formatData(20),
                Status = c.Status,
                CategoryId = c.CategoryId,
                Price = c.Price,
                CreationTime = c.CreationTime,
                LastModificationTime = c.LastModificationTime,
                Image = _storageService.GetFileUrl(c.Image),
                Content = c.Content,
                Description = c.Description,
                Id = c.Id,
                CategoryName = c.CategoryName.formatData(20),
                DiscountAmount = c.DiscountAmount,
                DiscountPercent = c.DiscountPercent,
                CreatedUserName = c.CreatedUserName
            }).ToList();
            var pagination = new Pagination<CourseViewModel>
            {
                Items = courses,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpGet("client-filter-no-pagination")]
        public async Task<IActionResult> GetCoursesNoPagingForClient(int? categoryId, long? priceMin, long? priceMax, string sortBy, string filter)
        {
            List<CourseViewModel> data;
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            if (categoryId != null)
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@categoryId", categoryId);
                var category = await _khoaHocDbContext.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);
                data = category.ParentId == null
                    ? (await conn.QueryAsync<CourseViewModel>("ListCoursesByCategoryParentId", parameters, null, 120,
                        CommandType.StoredProcedure)).ToList()
                    : (await conn.QueryAsync<CourseViewModel>("ListCoursesByCategoryId", parameters, null, 120,
                        CommandType.StoredProcedure)).ToList();
            }
            else
            {
                data = (await conn.QueryAsync<CourseViewModel>("ListCourses", null, null, 120,
                    CommandType.StoredProcedure)).ToList();
            }
            var query = data.AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.ToLower().Contains(filter.ToLower()) ||
                                         x.Name.convertToUnSign().ToLower().Contains(filter.convertToUnSign().ToLower()));
            }
            if (priceMin != null && priceMax != null)
            {
                query = query.Where(x => x.SortPrice >= priceMin && x.SortPrice <= priceMax);
            }
            query = sortBy switch
            {
                "name" => query.OrderByDescending(x => x.Name),
                "price_low_to_high" => query.OrderBy(x => x.SortPrice),
                "price_high_to_low" => query.OrderByDescending(x => x.SortPrice),
                _ => query.OrderByDescending(x => x.CreationTime)
            };
            data = query.ToList();
            foreach (var item in data)
            {
                if (item.DiscountPercent > 0)
                {
                    item.SortPrice = item.Price - (item.Price * item.DiscountPercent / 100);
                }
                else if (item.DiscountAmount > 0)
                {
                    item.SortPrice = item.DiscountAmount;
                }
                else
                {
                    item.SortPrice = item.Price;
                }
            }
            var courses = data.Select(c => new CourseViewModel()
            {
                Name = c.Name,
                Status = c.Status,
                CategoryId = c.CategoryId,
                Price = c.Price,
                CreationTime = c.CreationTime,
                LastModificationTime = c.LastModificationTime,
                Image = _storageService.GetFileUrl(c.Image),
                Content = c.Content,
                Description = c.Description,
                Id = c.Id,
                CategoryName = c.CategoryName,
                DiscountAmount = c.DiscountAmount,
                DiscountPercent = c.DiscountPercent,
                CreatedUserName = c.CreatedUserName
            }).ToList();
            return Ok(courses);
        }

        [HttpPut("{id}")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Update)]
        [ValidationFilter]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutCourse([FromForm] CourseCreateRequest request)
        {
            var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (course == null)
            {
                _logger.LogError($"Cannot found course with id {request.Id}");
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id {request.Id}"));
            }
            if (request.Image != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Image.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Image.OpenReadStream(), fileName, "images");
                course.Image = "images/" + fileName;
            }
            course.Name = request.Name;
            course.CategoryId = request.CategoryId;
            course.Status = request.Status;
            course.Description = request.Description;
            course.Content = request.Content;
            course.Price = request.Price;
            _khoaHocDbContext.Courses.Update(course);
            var sendUser = new AnnouncementToUserViewModel();
            int result;
            if (request.Status == 1)
            {
                var userCreated = await _userManager.FindByNameAsync(course.CreatedUserName);
                var userCurrent = await _userManager.FindByNameAsync(User.Identity.Name);
                var announceId = Guid.NewGuid();
                var announcement = new Announcement();
                announcement.UserId = userCurrent.Id;
                announcement.Status = 1;
                announcement.Content = $"{userCurrent.Name} đã duyệt khóa học {course.Name}";
                announcement.Title = "Khóa học đã được duyệt";
                announcement.Id = announceId;
                announcement.EntityId = course.Id.ToString();
                announcement.EntityType = "courses";
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
                sendUser.UserId = userCreated.Id.ToString();
                sendUser.AnnouncementViewModel = announcementViewmodel;
            }
            else
            {
                result = await _khoaHocDbContext.SaveChangesAsync();
            }
            if (result > 0)
            {
                return Ok(sendUser);
            }
            _logger.LogError("Update course failed");
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPut("approve")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> PutCourseApprove(List<int> input)
        {
            var lstAnnouncement = new List<AnnouncementToUserViewModel>();
            foreach (var id in input)
            {
                var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
                if (course == null)
                {
                    _logger.LogError($"Cannot found course with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id {id}"));
                }
                course.Status = 1;
                _khoaHocDbContext.Courses.Update(course);
                var userCreated = await _userManager.FindByNameAsync(course.CreatedUserName);
                var userCurrent = await _userManager.FindByNameAsync(User.Identity.Name);
                var announceId = Guid.NewGuid();
                var announcement = new Announcement();
                announcement.UserId = userCurrent.Id;
                announcement.Status = 1;
                announcement.Content = $"{userCurrent.Name} đã duyệt khóa học {course.Name}";
                announcement.Title = "Khóa học đã được duyệt";
                announcement.Id = announceId;
                announcement.EntityId = course.Id.ToString();
                announcement.EntityType = "courses";
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
                    _logger.LogError("Update course failed");
                    return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
                }
            }
            return Ok(lstAnnouncement);
        }

        [HttpPost("delete-multi-items")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Delete)]
        public async Task<IActionResult> DeleteCourse(List<int> input)
        {
            foreach (var id in input)
            {
                var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
                if (course == null)
                {
                    _logger.LogError($"Cannot found Course with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Cannot found Course with id {id}"));
                }
                var activeCourse = _khoaHocDbContext.ActivateCourses.Where(x => x.CourseId == course.Id);
                if (activeCourse.Any())
                {
                    foreach (var item in activeCourse)
                    {
                        var check = await _khoaHocDbContext.OrderDetails.FirstOrDefaultAsync(x => x.ActiveCourseId.Equals(item.Id));
                        if (check != null)
                        {
                            _logger.LogError($"Không thể xóa khóa học này với id là {item.Id}");
                            return BadRequest(new ApiBadRequestResponse($"Không thể xóa khóa học này với id là {item.Id}"));
                        }
                    }
                    _khoaHocDbContext.ActivateCourses.RemoveRange(activeCourse);
                }
                var lessons = _khoaHocDbContext.Lessons.Where(x => x.CourseId == id);
                if (lessons.Any())
                {
                    _khoaHocDbContext.Lessons.RemoveRange(lessons);
                }
                var comments = _khoaHocDbContext.Comments.Where(x => x.EntityId == course.Id && x.EntityType.Equals("Courses"));
                if (comments.Any())
                {
                    _khoaHocDbContext.Comments.RemoveRange(comments);
                }

                _khoaHocDbContext.Courses.Remove(course);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Delete Course failed");
            return BadRequest(new ApiBadRequestResponse("Xóa khóa học thất bại"));
        }

        [HttpGet("{id}/users")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        public async Task<IActionResult> GetCourseUsers(int id)
        {
            var courses = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (courses == null)
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id: {id}"));
            var activateCourses = _khoaHocDbContext.ActivateCourses.Include(x => x.AppUser)
                .Where(x => x.CourseId == courses.Id && x.Status).Select(u => new UserViewModel()
                {
                    UserName = u.AppUser.UserName,
                    Email = u.AppUser.Email,
                    PhoneNumber = u.AppUser.PhoneNumber,
                    Name = u.AppUser.Name,
                    Dob = u.AppUser.Dob,
                    Id = u.UserId.Value,
                    Avatar = u.AppUser.Avatar ?? "/img/defaultAvatar.png",
                    Biography = u.AppUser.Biography
                });
            return Ok(activateCourses);
        }

        [HttpPost("{coursesId}/users")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        public async Task<IActionResult> PostCourseUsers(int coursesId, List<string> request)
        {
            var courses = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == coursesId);
            if (courses == null)
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id: {coursesId}"));
            foreach (var activeCourse in request.Select(id => new ActivateCourse() { UserId = Guid.Parse(id), CourseId = coursesId, Status = true, Id = Guid.NewGuid() }))
            {
                await _khoaHocDbContext.ActivateCourses.AddAsync(activeCourse);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpPost("{coursesId}/users/delete-multi-items")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        public async Task<IActionResult> RemoveCourseUsers(int coursesId, List<string> request)
        {
            var courses = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == coursesId);
            if (courses == null)
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id: {coursesId}"));
            foreach (var id in request)
            {
                var activeCourse = _khoaHocDbContext.ActivateCourses.FirstOrDefault(x => x.CourseId == coursesId && x.UserId == Guid.Parse(id));
                _khoaHocDbContext.ActivateCourses.Remove(activeCourse!);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }

        [HttpPut("{coursesId}/users/active")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> PutActiveCourses(int coursesId, List<string> input)
        {
            var courses = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == coursesId);
            if (courses == null)
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id: {coursesId}"));
            foreach (var id in input)
            {
                var activeCourse = _khoaHocDbContext.ActivateCourses.FirstOrDefault(x => x.CourseId == coursesId && x.UserId == Guid.Parse(id));
                if (activeCourse == null)
                {
                    _logger.LogError($"Cannot found active course with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy mã kích hoạt với id {id}"));
                }
                activeCourse.Status = true;
                _khoaHocDbContext.ActivateCourses.Update(activeCourse);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Update active course failed");
            return BadRequest(new ApiBadRequestResponse("Update kích hoạt khóa học thất bại"));
        }

        [HttpGet("check-active-courses/user-{userId}")]
        public IActionResult GetActiveCourses(string userId)
        {
            var data = _khoaHocDbContext.ActivateCourses.Where(x => x.Status && x.UserId.Equals(Guid.Parse(userId)));
            var result = new List<ActiveCoursesViewModel>();
            foreach (var activateCourse in data)
            {
                var activateCourseVm = new ActiveCoursesViewModel();
                activateCourseVm.CourseId = activateCourse.CourseId;
                activateCourseVm.Status = activateCourse.Status;
                activateCourseVm.Id = activateCourse.Id.ToString();
                activateCourseVm.UserId = activateCourse.UserId.ToString();
                result.Add(activateCourseVm);
            }

            return Ok(result);
        }

        [HttpPut("user-active-course")]
        public async Task<IActionResult> PutActiveCourseForUser(ActiveCourseRequest request)
        {
            var key = _khoaHocDbContext.ActivateCourses.FirstOrDefault(x =>
                x.Id == Guid.Parse(request.Code));
            if (key == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy mã kích hoạt: {request.Code}"));
            }

            var checkCourse = _khoaHocDbContext.ActivateCourses.Any(x =>
                x.UserId.Equals(Guid.Parse(request.UserId)) && x.CourseId == key.CourseId);
            if (checkCourse)
            {
                return BadRequest(new ApiBadRequestResponse("Bạn đã có khóa học này rồi!"));
            }
            if (key.Status)
            {
                return BadRequest(new ApiBadRequestResponse("Mã kích hoạt này đã được sử dụng rồi"));
            }
            key.Status = true;
            key.UserId = Guid.Parse(request.UserId);
            _khoaHocDbContext.ActivateCourses.Update(key);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }

            return BadRequest(new ApiBadRequestResponse("Không tìm thấy mã kích hoạt này"));
        }

        [HttpGet("my-courses/user-{userId}")]
        public async Task<IActionResult> GetCoursesByUserPaging(string userId, int pageIndex, int pageSize)
        {
            var items = new List<CourseViewModel>();
            var query = _khoaHocDbContext.ActivateCourses.Include(x => x.Course)
                .AsNoTracking()
                .Where(x => x.UserId.Equals(Guid.Parse(userId)) && x.Status);
            var totalRecords = await query.CountAsync();
            var result = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            foreach (var activateCourse in result)
            {
                var course = activateCourse.Course;
                var category = _khoaHocDbContext.Categories.FirstOrDefault(x => x.Id == activateCourse.Course.CategoryId);
                var user = await _userManager.FindByNameAsync(activateCourse.Course.CreatedUserName);
                items.Add(new CourseViewModel
                {
                    Name = course.Name,
                    Status = course.Status,
                    LastModificationTime = course.LastModificationTime,
                    CategoryId = course.CategoryId,
                    CategoryName = category?.Name,
                    CreationTime = course.CreationTime,
                    Image = _storageService.GetFileUrl(course.Image),
                    Content = course.Content,
                    CreatedName = user.Name,
                    Id = course.Id,
                    Description = course.Description
                });
            }
            var pagination = new Pagination<CourseViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpGet("{courseId}/active-courses")]
        public async Task<IActionResult> GetActiveCourses(int coursesId)
        {
            var data = await _khoaHocDbContext.ActivateCourses.Where(x => x.CourseId == coursesId && x.Status).Select(x => new ActiveCoursesViewModel()
            {
                Status = x.Status,
                CourseId = x.CourseId,
                UserId = x.UserId.ToString(),
                Id = x.UserId.ToString()
            }).ToListAsync();
            return Ok(data);
        }

        [HttpPost("create-active-course")]
        public async Task<IActionResult> PostActiveCourse(ActiveCourseCreateRequest request)
        {
            var key = Guid.NewGuid();
            var active = new ActivateCourse();
            active.CourseId = request.CourseId;
            active.Status = request.Status;
            active.Id = key;
            if (!string.IsNullOrEmpty(request.UserId))
            {
                active.UserId = Guid.Parse(request.UserId);
            }
            await _khoaHocDbContext.ActivateCourses.AddAsync(active);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok(key);
            }

            return BadRequest("Không thể tạo mã kích hoạt");
        }

        [HttpDelete("{id}/image")]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
                return BadRequest(new ApiBadRequestResponse($"Không thể tìm thấy khóa học với id {id}"));
            course.Image = "";
            _khoaHocDbContext.Courses.Update(course);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }
    }
}