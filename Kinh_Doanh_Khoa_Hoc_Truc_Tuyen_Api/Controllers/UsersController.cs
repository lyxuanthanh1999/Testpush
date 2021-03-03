using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
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

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("Bearer")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly ILogger<UsersController> _logger;

        private readonly IStorageService _storageService;

        public UsersController(UserManager<AppUser> userManager, EKhoaHocDbContext khoaHocDbContext, RoleManager<AppRole> roleManager, ILogger<UsersController> logger, IStorageService storageService)
        {
            _userManager = userManager;
            _khoaHocDbContext = khoaHocDbContext;
            _roleManager = roleManager;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _storageService = storageService;
        }

        [HttpPost]
        [ValidationFilter]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostUser([FromForm] UserCreateRequest request)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Dob = DateTime.Parse(request.Dob),
                UserName = request.UserName,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Biography = request.Biography
            };
            if (request.Avatar != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Avatar.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Avatar.OpenReadStream(), fileName, "images");
                user.Avatar = "images/" + fileName;
            }
            else
            {
                user.Avatar = "images/defaultAvatar.png";
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var userCheck = await _userManager.FindByNameAsync(request.UserName);
                        await _userManager.AddToRoleAsync(userCheck, "Student");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ApiBadRequestResponse(e.Message));
                    }
                }
                return Ok();
            }
            _logger.LogError("Create user failed");
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpPost("server")]
        [ValidationFilter]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PostUserByAdmin([FromForm] UserCreateRequest request)
        {
            var user = new AppUser
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Dob = DateTime.Parse(request.Dob),
                UserName = request.UserName,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Biography = request.Biography,
                EmailConfirmed = true
            };
            if (request.Avatar != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Avatar.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Avatar.OpenReadStream(), fileName, "images");
                user.Avatar = "images/" + fileName;
            }
            else
            {
                user.Avatar = "images/defaultAvatar.png";
            }
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var userCheck = await _userManager.FindByNameAsync(request.UserName);
                        await _userManager.AddToRoleAsync(userCheck, "Student");
                    }
                    catch (Exception e)
                    {
                        return BadRequest(new ApiBadRequestResponse(e.Message));
                    }
                }
                return Ok();
            }
            _logger.LogError("Create user failed");
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            if (result == null)
            {
                _logger.LogError($"Cannot found user with id: {id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {id}"));
            }

            return Ok(new UserViewModel
            {
                Dob = result.Dob,
                Email = result.Email,
                Id = result.Id,
                Name = result.Name,
                PhoneNumber = result.PhoneNumber,
                UserName = result.UserName,
                Avatar = result.Avatar == null ? "images/defaultAvatar.png" : _storageService.GetFileUrl(result.Avatar),
                Biography = result.Biography
            });
        }

        [HttpGet("user-{userName}")]
        public async Task<IActionResult> GetByUserName(string userName)
        {
            var result = await _userManager.FindByNameAsync(userName);
            if (result == null)
            {
                _logger.LogError($"Cannot found user with userName: {userName}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với username {userName}"));
            }

            return Ok(new UserViewModel
            {
                Dob = result.Dob,
                Email = result.Email,
                Id = result.Id,
                Name = result.Name,
                PhoneNumber = result.PhoneNumber,
                UserName = result.UserName,
                Avatar = result.Avatar == null ? "images/defaultAvatar.png" : _storageService.GetFileUrl(result.Avatar),
                Biography = result.Biography
            });
        }

        [HttpGet("email-{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result == null)
            {
                _logger.LogError($"Cannot found user with email: {email}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với email: {email}"));
            }

            return Ok(new UserViewModel
            {
                Dob = result.Dob,
                Email = result.Email,
                Id = result.Id,
                Name = result.Name,
                PhoneNumber = result.PhoneNumber,
                UserName = result.UserName,
                Avatar = result.Avatar == null ? "images/defaultAvatar.png" : _storageService.GetFileUrl(result.Avatar),
                Biography = result.Biography,
                ConfirmEmail = result.EmailConfirmed
            });
        }

        [HttpGet("{email}-reset-password-token")]
        public async Task<IActionResult> GetPasswordResetToken(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogError($"Cannot found user with email: {email}");
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {email}"));
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                return Ok(code);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiBadRequestResponse(e.Message));
            }
        }

        [HttpPost("reset-password-confirm")]
        public async Task<IActionResult> PostForgotPasswordConfirmation(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {request.UserId}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {request.UserId}"));
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
            }

            return Ok();
        }

        [HttpGet("token-user-{id}")]
        public async Task<IActionResult> GetEmailConfirmationToken(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogError($"Cannot found user with id: {id}");
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {id}"));
                }

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                return Ok(code);
            }
            catch (Exception e)
            {
                return BadRequest(new ApiBadRequestResponse(e.Message));
            }
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> PostEmailConfirmation([FromBody] ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {request.UserId}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {request.UserId}"));
            }

            var result = await _userManager.ConfirmEmailAsync(user, request.Code);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
            }

            return Ok();
        }

        [HttpGet("course-{id}")]
        public async Task<IActionResult> GetByCourseId(int id)
        {
            var course = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                _logger.LogError($"Cannot found course with id: {id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy khóa học với id: {id}"));
            }
            var result = await _userManager.FindByNameAsync(course.CreatedUserName);
            if (result == null)
            {
                _logger.LogError($"Cannot found user with userName: {course.CreatedUserName}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với username {course.CreatedUserName}"));
            }
            var countStudents = _khoaHocDbContext.ActivateCourses.Include(x => x.Course).Count(x => x.Course.CreatedUserName.Equals(result.UserName) && x.Id != result.Id && x.Status);
            var countCourses = _khoaHocDbContext.Courses.Count(x => x.CreatedUserName.Equals(result.UserName));
            return Ok(new UserViewModel
            {
                Dob = result.Dob,
                Email = result.Email,
                Id = result.Id,
                Name = result.Name,
                PhoneNumber = result.PhoneNumber,
                UserName = result.UserName,
                Avatar = result.Avatar == null ? "images/defaultAvatar.png" : _storageService.GetFileUrl(result.Avatar),
                Biography = result.Biography,
                CountCourses = countCourses,
                CountStudents = countStudents
            });
        }

        [HttpGet]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.View)]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _userManager.Users.Select(x => new UserViewModel
            {
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Dob = x.Dob,
                Id = x.Id,
                Avatar = x.Avatar == null ? "images/defaultAvatar.png" : _storageService.GetFileUrl(x.Avatar),
                Biography = x.Biography
            }).ToListAsync();
            if (user.Any())
            {
                return Ok(user);
            }
            _logger.LogError("Cannot found user");
            return NotFound(new ApiNotFoundResponse("Cannot found user"));
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.View)]
        public IActionResult GetUsersPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _userManager.Users.AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.Email.ToLower().Equals(filter.ToLower()) || x.UserName.ToLower().Contains(filter.ToLower()) ||
                    x.Name.ToLower().Contains(filter.ToLower()) ||
                    x.Name.convertToUnSign().ToLower().Contains(filter.convertToUnSign().ToLower()) ||
                    x.PhoneNumber.Contains(filter));
            }

            var data = query.ToList();
            var totalRecords = data.Count();
            var items = data.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(u => new UserViewModel
            {
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Name = u.Name,
                Dob = u.Dob,
                Id = u.Id,
                Avatar = u.Avatar == null ? "images/defaultAvatar.png" : _storageService.GetFileUrl(u.Avatar),
                Biography = u.Biography
            }).ToList();
            var pagination = new Pagination<UserViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpPost("{id}/check-password")]
        public async Task<IActionResult> PostUserCheckPassword(AccountPasswordCheckRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {request.Id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {request.Id}"));
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Mật khẩu hiện tại không đúng với hệ thống");
        }

        [HttpPut("{id}")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.Update)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutUser(string id, [FromForm] UserCreateRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {id}"));
            }
            user.Name = request.Name;
            user.Dob = DateTime.Parse(request.Dob);
            if (request.Avatar != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Avatar.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Avatar.OpenReadStream(), fileName, "images");
                user.Avatar = "images/" + fileName;
            }
            user.Biography = request.Biography;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            _logger.LogError("Update user failed");
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPut("information-{id}")]
        public async Task<IActionResult> PutInformationUser(string id, [FromBody] UserCreateRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {id}"));
            }
            user.Name = request.Name;
            user.Dob = DateTime.Parse(request.Dob);
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            _logger.LogError("Update user failed");
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPut("{id}/change-avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> PutUserChangeAvatar([FromForm] AccountChangeAvatarRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {request.Id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {request.Id}"));
            }
            if (request.Avatar != null)
            {
                var originalFileName = ContentDispositionHeaderValue.Parse(request.Avatar.ContentDisposition).FileName.Trim('"');
                var fileName = $"{originalFileName.Substring(0, originalFileName.LastIndexOf('.'))}{Path.GetExtension(originalFileName)}";
                await _storageService.SaveFileAsync(request.Avatar.OpenReadStream(), fileName, "images");
                user.Avatar = "images/" + fileName;
            }
            else
            {
                user.Avatar = "images/defaultAvatar.png";
            }
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return NoContent();
            }
            _logger.LogError("Update user failed");
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPut("{id}/change-password")]
        [ValidationFilter]
        public async Task<IActionResult> PutUserPassword([FromBody] UserPasswordChangeRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {request.Id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {request.Id}"));
            }

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return NoContent();
            }

            _logger.LogError("Change password failed");
            return BadRequest(new ApiBadRequestResponse("Đổi mật khẩu thất bại"));
        }

        [HttpPost("delete-multi-items")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.Delete)]
        [ValidationFilter]
        public async Task<IActionResult> DeleteUser(List<string> ids)
        {
            if (ids.Contains(User.GetUserId()))
            {
                return BadRequest(new ApiBadRequestResponse("Bạn không thể xóa chính tài khoản bạn đang sử dụng được"));
            }
            foreach (var id in ids)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogError($"Cannot found user with id: {id}");
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {id}"));
                }
                var adminUsers = await _userManager.GetUsersInRoleAsync(SystemConstants.Admin);
                if (adminUsers.All(x => x.Id == Guid.Parse(id)))
                {
                    _logger.LogError("You cannot remove the only admin user remaining.");
                    return BadRequest(new ApiBadRequestResponse("Bạn không thể xóa hết tất cả user có quyền admin"));
                }
                var orders = _khoaHocDbContext.Orders.Where(x => x.UserId.Equals(user.Id));
                if (orders.Any())
                {
                    _logger.LogError("You cannot remove orders.");
                    return BadRequest(new ApiBadRequestResponse("Bạn không thể xóa user này vì tài khoản này vì user này vì tài khoản này có hóa đơn đặt hàng"));
                }
                var courses = _khoaHocDbContext.Courses.Where(x => x.CreatedUserName.Equals(user.UserName));
                if (courses.Any())
                {
                    _logger.LogError("You cannot remove courses.");
                    return BadRequest(new ApiBadRequestResponse("Bạn không thể xóa user này vì tài khoản này vì tài khoản này có khóa học"));
                }
                var activeCourses = _khoaHocDbContext.ActivateCourses.Where(x => x.UserId.Equals(user.Id));
                if (activeCourses.Any())
                {
                    _logger.LogError("You cannot remove activeCourses.");
                    return BadRequest(new ApiBadRequestResponse("Bạn không thể xóa user này vì tài khoản này vì user này có mã khóa học"));
                }
                var announcements = _khoaHocDbContext.Announcements.Where(x => x.UserId.Equals(user.Id));
                if (announcements.Any())
                {
                    var announcementUsers = _khoaHocDbContext.AnnouncementUsers.Where(x => x.UserId.Equals(user.Id));
                    _khoaHocDbContext.Announcements.RemoveRange(announcements);
                    _khoaHocDbContext.AnnouncementUsers.RemoveRange(announcementUsers);
                }
                var roles = _roleManager.Roles;
                foreach (var role in roles)
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("Delete user failed");
                    return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
                }
            }

            return Ok();
        }

        [HttpGet("{id}/administrator/reset-password")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> ResetPasswordForAdmin(string id)
        {
            if (id.Equals(User.GetUserId()))
            {
                return BadRequest(new ApiBadRequestResponse("Bạn không thể khôi phục mật khẩu tài khoản bạn đang sử dụng được"));
            }
            var user = await _userManager.FindByIdAsync(id);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, "123@qwe");
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Khôi phục password thất bại"));
        }

        [HttpDelete("{id}-delete-avatar")]
        public async Task<IActionResult> DeleteAvatar(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogError($"Cannot found user with id: {id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {id}"));
            }
            user.Avatar = "";
            _khoaHocDbContext.Users.Update(user);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }

        [HttpGet("{userId}/menu")]
        public async Task<IActionResult> GetMenuByUserPermission(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            var query = from f in _khoaHocDbContext.Functions
                        join p in _khoaHocDbContext.Permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        join c in _khoaHocDbContext.Commands on p.CommandId equals c.Id
                        where roles.Contains(r.Name) && c.Id == "View"
                        select new FunctionViewModel
                        {
                            Id = f.Id,
                            Name = f.Name,
                            ParentId = f.ParentId,
                            SortOrder = f.SortOrder,
                            Url = f.Url,
                            Icon = f.Icon
                        };
            var data = await query.Distinct()
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.SortOrder)
                .ToListAsync();
            return Ok(data);
        }

        [HttpGet("{userId}/roles")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.View)]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {userId}"));
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("{userId}/roles")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.View)]
        public async Task<IActionResult> PostRolesToUserUser(string userId, [FromBody] RoleAssignRequest request)
        {
            if (request.RoleNames?.Length == 0)
            {
                return BadRequest(new ApiBadRequestResponse("Tên quyền không được để trống"));
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {userId}"));
            var result = await _userManager.AddToRolesAsync(user, request.RoleNames);
            if (result.Succeeded)
                return Ok();

            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpDelete("{userId}/roles")]
        [ClaimRequirement(FunctionConstant.User, CommandConstant.View)]
        public async Task<IActionResult> RemoveRolesFromUser(string userId, [FromQuery] RoleAssignRequest request)
        {
            if (request.RoleNames?.Length == 0)
            {
                return BadRequest(new ApiBadRequestResponse("Tên quyền không được để trống"));
            }
            if (request.RoleNames!.Contains(SystemConstants.Admin))
            {
                if ((await _userManager.GetUsersInRoleAsync("Admin")).Count < 2)
                {
                    return BadRequest(new ApiBadRequestResponse($"Không thể xóa quyền {SystemConstants.Admin}"));
                }
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy user với id: {userId}"));
            var result = await _userManager.RemoveFromRolesAsync(user, request.RoleNames);
            if (result.Succeeded)
                return Ok();

            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }
    }
}