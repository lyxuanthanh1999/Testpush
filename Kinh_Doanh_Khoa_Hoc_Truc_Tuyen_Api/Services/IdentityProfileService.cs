using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _claimsPrincipalFactory;

        private readonly UserManager<AppUser> _userManager;

        private readonly EKhoaHocDbContext _applicationDbContext;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly IStorageService _storageService;

        public IdentityProfileService(RoleManager<AppRole> roleManager, EKhoaHocDbContext applicationDbContext, UserManager<AppUser> userManager, IUserClaimsPrincipalFactory<AppUser> claimsPrincipalFactory, IStorageService storageService)
        {
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _storageService = storageService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsPrincipalFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();
            var roles = await _userManager.GetRolesAsync(user);
            var query = from p in _applicationDbContext.Permissions
                        join c in _applicationDbContext.Commands on p.CommandId equals c.Id
                        join f in _applicationDbContext.Functions on p.FunctionId equals f.Id
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name)
                        select f.Id + "_" + c.Id;
            var permissions = await query.Distinct().ToListAsync();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, string.Join(";", roles)));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(SystemConstants.Permissions, JsonConvert.SerializeObject(permissions)));
            claims.Add(new Claim("FullName", user.Name));
            claims.Add(new Claim("Avatar", _storageService.GetFileUrl(user.Avatar)));
            context.IssuedClaims = claims;

            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0123456789ABCDEF"));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //context.
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}