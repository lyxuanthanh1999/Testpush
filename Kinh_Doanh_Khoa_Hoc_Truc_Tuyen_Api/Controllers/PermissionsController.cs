using Dapper;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PermissionsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        // [ClaimRequirement(FunctionConstant.Permission, CommandConstant.View)]
        public async Task<IActionResult> GetCommandViews()
        {
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var query = @"SELECT f.Id,
	                       f.Name,
	                       f.ParentId,
	                       sum(case when sa.Id = 'Create' then 1 else 0 end) as HasCreate,
	                       sum(case when sa.Id = 'Update' then 1 else 0 end) as HasUpdate,
	                       sum(case when sa.Id = 'Delete' then 1 else 0 end) as HasDelete,
	                       sum(case when sa.Id = 'View' then 1 else 0 end) as HasView,
	                       sum(case when sa.Id = 'ExportExcel' then 1 else 0 end) as HasExportExcel,
                           sum(case when sa.Id = 'Approve' then 1 else 0 end) as HasApprove
                        from Functions f join CommandInFunctions cif on f.Id = cif.FunctionId
		                    left join Commands sa on cif.CommandId = sa.Id
                        GROUP BY f.Id,f.Name, f.ParentId
                        order BY f.ParentId";
            var result = await conn.QueryAsync<PermissionScreenViewModel>(query, null, null, 120, CommandType.Text);

            return Ok(result.ToList());
        }
    }
}