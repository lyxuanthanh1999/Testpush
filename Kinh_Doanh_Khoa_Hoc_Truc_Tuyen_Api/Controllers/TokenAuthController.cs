using IdentityModel.Client;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenAuthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly UserManager<AppUser> _userManager;

        private readonly SignInManager<AppUser> _signInManager;

        public TokenAuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate(LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return NotFound(new ApiNotFoundResponse("Username không tồn tại"));
            var serverClient = _httpClientFactory.CreateClient();

            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:44342/");

            var tokenResponse = await serverClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = model.ClientId,
                    ClientSecret = model.ClientSecret,
                    Scope = model.Scope,
                    UserName = model.UserName,
                    Password = model.Password
                });
            if (tokenResponse.IsError)
            {
                return BadRequest(new ApiBadRequestResponse("Tài khoản hoặc mật khẩu không đúng"));
            }
            return Ok(new TokenResponseFromServer()
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                TokenType = tokenResponse.TokenType
            });
        }
    }
}