using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Microsoft.AspNetCore.Mvc;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(FunctionConstant functionId, CommandConstant commandId)
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { functionId, commandId };
        }
    }
}