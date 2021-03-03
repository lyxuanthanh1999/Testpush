using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        private readonly FunctionConstant _functionCode;

        private readonly CommandConstant _commandCode;

        public ClaimRequirementFilter(FunctionConstant functionCode, CommandConstant commandCode)
        {
            _functionCode = functionCode;
            _commandCode = commandCode;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var check = context.HttpContext.User;
            var check2 = context.HttpContext.User.Claims;
            var permissionsClaim = context.HttpContext.User.Claims
                .SingleOrDefault(c => c.Type == SystemConstants.Permissions);
            if (permissionsClaim != null)
            {
                var permissions = JsonConvert.DeserializeObject<List<string>>(permissionsClaim.Value);
                if (!permissions.Contains(_functionCode + "_" + _commandCode))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
}