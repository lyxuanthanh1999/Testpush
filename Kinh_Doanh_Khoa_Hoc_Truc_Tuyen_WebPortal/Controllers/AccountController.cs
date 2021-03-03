using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaulMiami.AspNetCore.Mvc.Recaptcha;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IBaseApiClient _apiClient;

        private readonly IConfiguration _configuration;

        private readonly IEmailSender _emailSender;

        public AccountController(IBaseApiClient apiClient,
            IConfiguration configuration, IEmailSender emailSender)
        {
            _apiClient = apiClient;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Route("my-profile.html")]
        public async Task<IActionResult> AccountDetail()
        {
            var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/{User.GetUserId()}");
            var account = new AccountDetailViewModel();
            account.AccountViewModel = new AccountViewModel
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Dob = user.Dob,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id
            };
            account.Image = user.Avatar;
            account.NameDash = "My Profile";
            return View(account);
        }

        [HttpGet]
        [Route("my-announcements.html")]
        public async Task<IActionResult> MyAnnouncements(int? pageSize, string filterBy = "true", int page = 1)
        {
            pageSize ??= 4;
            var announce = await _apiClient.GetAsync<Pagination<AnnouncementViewModel>>($"/api/announcements/private-paging/filter?userId={User.GetUserId()}&pageSize={pageSize}&pageIndex={page}&filter={filterBy}");
            var data = new MyAnnouncementViewModel();
            data.TmpPage = page;
            data.FilterType = filterBy;
            data.NameDash = "My Announcements";
            data.AnnouncementViewModels = announce;
            return View(data);
        }

        [HttpGet]
        [Route("check-my-announcement.html")]
        public async Task<IActionResult> CheckMyAnnouncement(string announceId, int? pageSize, string filterBy = "true", int page = 1)
        {
            try
            {
                pageSize ??= 4;
                await _apiClient.PutAsync($"/api/announcements/mark-read", new AnnouncementMarkReadRequest { AnnounceId = announceId, UserId = User.GetUserId() });
                return RedirectToAction(nameof(MyAnnouncements), new { pageSize, filterBy, page });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("my-courses.html")]
        public async Task<IActionResult> MyCourses(int? pageSize, int page = 1)
        {
            pageSize ??= 3;
            var data = new MyCoursesViewModel();
            data.PageSize = pageSize;
            data.CourseViewModels = await _apiClient.GetAsync<Pagination<CourseViewModel>>($"/api/courses/my-courses/user-{User.GetUserId()}?userId={User.GetUserId()}&pageIndex={page}&pageSize={pageSize}");
            data.NameDash = "My Courses";
            return View(data);
        }

        [HttpGet]
        [Route("active-course.html")]
        public IActionResult ActiveCourse()
        {
            var data = new ActiveCourseViewModel();
            data.NameDash = "Active Course";
            return View(data);
        }

        [HttpPost]
        [Route("active-course.html")]
        public async Task<IActionResult> ActiveCourse(ActiveCourseViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Mã kích hoạt không thể để trống");
                    request.NameDash = "Active Course";
                    return View(request);
                }

                var active = new ActiveCourseRequest
                {
                    Code = request.Code,
                    UserId = User.GetUserId()
                };
                await _apiClient.PutAsync($"/api/courses/user-active-course", active);
                return RedirectToAction(nameof(ActiveCourseConfirmation));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                request.NameDash = "Active Course";
                return View(request);
            }
        }

        [HttpGet]
        [Route("active-course-confirm.html")]
        public IActionResult ActiveCourseConfirmation()
        {
            var data = new ActiveCourseViewModel();
            data.NameDash = "Active Course";
            return View(data);
        }

        [HttpGet]
        [Route("manage-orders.html")]
        public async Task<IActionResult> ManageOrder(int? pageSize, string sortBy = "all", int page = 1)
        {
            pageSize ??= 3;
            var order = await _apiClient.GetAsync<Pagination<OrderViewModel>>($"/api/orders/account-{User.GetUserId()}-orders?userId={User.GetUserId()}&pageSize={pageSize}&pageIndex={page}&sortBy={sortBy}");
            var data = new ManageOrderViewModel();
            data.SortType = sortBy;
            data.NameDash = "My Order";
            data.OrderViewModels = order;
            return View(data);
        }

        [HttpGet]
        [Route("track-order-detail.html")]
        public async Task<IActionResult> TrackOrderDetail(int orderId)
        {
            var order = await _apiClient.GetAsync<OrderViewModel>($"/api/orders/{orderId}/user-{User.GetUserId()}");
            var data = new TrackOrderDetailViewModel();
            data.NameDash = "Track Order";
            data.OrderViewModel = order;
            return View(data);
        }

        [HttpGet]
        [Route("my-edit-profile.html")]
        public async Task<IActionResult> EditAccountDetail()
        {
            var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/{User.GetUserId()}");
            var account = new AccountDetailViewModel();
            account.AccountViewModel = new AccountViewModel
            {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Dob = user.Dob,
                PhoneNumber = user.PhoneNumber,
                Id = user.Id,
            };
            account.Image = user.Avatar;
            account.NameDash = "My Profile";
            return View(account);
        }

        [HttpPost]
        [Route("my-edit-profile.html")]
        public async Task<IActionResult> EditAccountDetail(AccountDetailViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var user = new UserViewModel();
            user.Name = request.AccountViewModel.Name;
            user.Dob = request.AccountViewModel.Dob;
            user.Email = request.AccountViewModel.Email;
            user.PhoneNumber = request.AccountViewModel.PhoneNumber;
            user.UserName = request.AccountViewModel.UserName;
            await _apiClient.PutAsync($"/api/users/information-{User.GetUserId()}", user);
            return RedirectToAction(nameof(AccountDetail));
        }

        [HttpPost]
        [Route("my-edit-avatar.html")]
        public async Task<IActionResult> EditAvatar(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(EditAccountDetail));
            }

            var requestContent = new MultipartFormDataContent();
            if (file != null)
            {
                byte[] data;
                using (var br = new BinaryReader(file.OpenReadStream()))
                {
                    data = br.ReadBytes((int)file.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Avatar", file.FileName);
            }
            requestContent.Add(new StringContent(User.GetUserId()), "Id");
            await _apiClient.PutForFileAsync<bool>($"/api/users/{User.GetUserId()}/change-avatar", requestContent);
            return RedirectToAction(nameof(EditAccountDetail));
        }

        [HttpGet]
        [Route("change-my-password.html")]
        public IActionResult EditAccountPassword()
        {
            var account = new ChangePasswordViewModel();
            account.Id = User.GetUserId();
            account.NameDash = "My Profile";
            return View(account);
        }

        [HttpPost]
        [Route("change-my-password.html")]
        public async Task<IActionResult> EditAccountPassword(ChangePasswordViewModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    request.NameDash = "My Profile";
                    return View(request);
                }
                var checkPassword = new AccountPasswordCheckRequest
                {
                    Password = request.OldPassword,
                    Id = request.Id
                };
                await _apiClient.PostReturnBooleanAsync(
                        $"/api/users/{request.Id}/check-password", checkPassword);
                var data = new UserPasswordChangeRequest();
                data.NewPassword = request.NewPassword;
                data.CurrentPassword = request.OldPassword;
                data.Id = request.Id;

                await _apiClient.PutAsync($"/api/users/{data.Id}/change-password", data);
                return RedirectToAction(nameof(AccountDetail));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                request.NewPassword = null;
                request.ConfirmNewPassword = null;
                request.OldPassword = null;
                request.NameDash = "My Profile";
                return View(request);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login.html")]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login.html")]
        public async Task<IActionResult> Login(LoginRequest request, string returnUrl = null)
        {
            try
            {
                ViewData["ReturnUrl"] = returnUrl;
                if (!ModelState.IsValid)
                    return View(request);
                var loginViewModel = new LoginViewModel()
                {
                    ClientId = _configuration["Authorization:ClientId"],
                    ClientSecret = _configuration["Authorization:ClientSecret"],
                    Scope = _configuration["Authorization:Scope"],
                    Password = request.Password,
                    UserName = request.UserName,
                    RememberMe = request.RememberMe
                };
                var result = await _apiClient.PostAsync<LoginViewModel, TokenResponseFromServer>($"/api/TokenAuth/Authenticate", loginViewModel, false);
                var principal = ValidateToken(result);
                if (principal.IsInRole("Teacher"))
                {
                    ModelState.AddModelError("", "Không Thể Đăng Nhập Tài Khoản Này");
                    return View();
                }
                var authProperties = new AuthenticationProperties();
                if (request.RememberMe)
                {
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
                    authProperties.IsPersistent = true;
                }
                else
                {
                    authProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10);
                    authProperties.IsPersistent = false;
                }
                HttpContext.Session.Remove(SystemConstants.AttachmentSession);
                HttpContext.Session.Remove(SystemConstants.EmailSession);
                HttpContext.Session.Remove(SystemConstants.CartSession);
                HttpContext.Session.Set("access_token", result.AccessToken);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    authProperties);
                return RedirectToLocal(returnUrl);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("register.html")]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ValidateRecaptcha]
        [Route("register.html")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(registerRequest);
                }
                var requestContent = new MultipartFormDataContent();
                if (registerRequest.Avatar != null)
                {
                    byte[] data;
                    using (var br = new BinaryReader(registerRequest.Avatar.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)registerRequest.Avatar.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "Avatar", registerRequest.Avatar.FileName);
                }
                requestContent.Add(new StringContent(registerRequest.UserName), "UserName");
                requestContent.Add(new StringContent(registerRequest.Password), "Password");
                requestContent.Add(new StringContent(registerRequest.Email), "Email");
                requestContent.Add(new StringContent(registerRequest.PhoneNumber), "PhoneNumber");
                requestContent.Add(new StringContent(registerRequest.Name), "Name");
                requestContent.Add(new StringContent(registerRequest.Dob.ToString("yyyy/MM/dd")), "Dob");
                await _apiClient.PostForFileAsync<bool>("/api/users", requestContent, false);
                var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/user-{registerRequest.UserName}");
                var code = await _apiClient.GetStringAsync($"/api/users/token-user-{user.Id.ToString()}");
                var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailConfirmationAsync(registerRequest.Email, callbackUrl);
                return RedirectToAction(nameof(RegisterConfirmation));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Email hoặc tài khoản đã tồn tại");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/{userId}");
            if (user == null)
            {
                throw new ApplicationException($"Không thể load user với id '{userId}'.");
            }

            var result = await _apiClient.PostReturnBooleanAsync($"/api/users/confirm-email", new ConfirmEmailRequest
            {
                Code = code,
                UserId = userId
            }, false);
            return View(result ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("reset-password.html")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("reset-password.html")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/email-{model.Email}");
                if (user == null || !(user.ConfirmEmail))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var token = await _apiClient.GetStringAsync($"/api/users/{model.Email}-reset-password-token");
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, token, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Khôi phục mật khẩu",
                   $"Bạn hãy nhấn vào đây để khôi phục mật khẩu: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }
            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(Guid userId, string token)
        {
            if (token == null || userId == null)
            {
                throw new ApplicationException("Token vs ID must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { UserId = userId, Token = token };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var user = await _apiClient.GetAsync<UserViewModel>($"/api/users/{model.UserId}");
                if (user == null)
                {
                    // Don't reveal that the user does not exist
                    return RedirectToAction(nameof(ResetPasswordConfirmation));
                }
                await _apiClient.PostReturnBooleanAsync($"/api/users/reset-password-confirm", new ResetPasswordRequest
                {
                    Password = model.Password,
                    Token = model.Token,
                    UserId = model.UserId.ToString()
                }, false);
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private ClaimsPrincipal ValidateToken(TokenResponseFromServer result)
        {
            var stream = result.AccessToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;
            var claims = new List<Claim>
            {
                new Claim("access_token", result.AccessToken),
                new Claim("refresh_token", result.RefreshToken),
                new Claim("token_type", result.TokenType),
                new Claim("expires_in", result.ExpiresIn.ToString())
            };
            claims.AddRange(tokenS!.Claims);
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            return principal;
        }

        #endregion Helpers

        #region Ajax Method

        public async Task<IActionResult> GetAnnouncements(int? pageSize, string filterBy = "false", int page = 1)
        {
            var userId = ((ClaimsIdentity)User.Identity)?.Claims
                .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            pageSize ??= 3;
            var announce = await _apiClient.GetAsync<Pagination<AnnouncementViewModel>>($"/api/announcements/private-paging/filter?userId={userId}&pageSize={pageSize}&pageIndex={page}&filter={filterBy}");
            return new OkObjectResult(announce);
        }

        public async Task<IActionResult> MarkAsRead(string announceId)
        {
            await _apiClient.PutAsync($"/api/announcements/mark-read", new AnnouncementMarkReadRequest { AnnounceId = announceId, UserId = User.GetUserId() });
            return Ok();
        }

        #endregion Ajax Method
    }
}