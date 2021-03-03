using Dapper;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<StatisticsController> _logger;

        public readonly UserManager<AppUser> _userManager;

        private readonly IConfiguration _configuration;

        public StatisticsController(EKhoaHocDbContext khoaHocDbContext, ILogger<StatisticsController> logger, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet("new-register")]
        [ClaimRequirement(FunctionConstant.NewUser, CommandConstant.View)]
        public async Task<IActionResult> GetNewRegisters(int key, string dateFrom, string dateTo)
        {
            if (key == 1)
            {
                var now = DateTime.Now;
                var sevenDayAgo = now.Date.AddDays(-7);
                dateFrom = sevenDayAgo.ToString("yyyy/MM/dd");
                dateTo = now.ToString("yyyy/MM/dd");
            }
            else if (key == 2)
            {
                var now = DateTime.Now;
                var monthDayAgo = now.Date.AddDays(-30);
                dateFrom = monthDayAgo.ToString("yyyy/MM/dd");
                dateTo = now.ToString("yyyy/MM/dd");
            }

            var data = await _khoaHocDbContext.Users.Where(x => x.CreationTime.Date >= DateTime.Parse(dateFrom).Date && x.CreationTime.Date <= DateTime.Parse(dateTo).Date)
                .GroupBy(x => x.CreationTime.Date)
                .Select(g => new DateStatisticViewModel()
                {
                    Date = g.Key.ToString(),
                    NumberOfValue = g.Count()
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("revenue-daily")]
        [ClaimRequirement(FunctionConstant.Revenue, CommandConstant.View)]
        public async Task<IActionResult> GetRevenueDaily(int key, string dateFrom, string dateTo)
        {
            if (key == 1)
            {
                var now = DateTime.Now;
                var sevenDayAgo = now.Date.AddDays(-7);
                dateFrom = sevenDayAgo.ToString("yyyy/MM/dd");
                dateTo = now.ToString("yyyy/MM/dd");
            }
            else if (key == 2)
            {
                var now = DateTime.Now;
                var monthDayAgo = now.Date.AddDays(-30);
                dateFrom = monthDayAgo.ToString("yyyy/MM/dd");
                dateTo = now.ToString("yyyy/MM/dd");
            }

            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@fromDate", dateFrom);
            dynamicParameters.Add("@toDate", dateTo);
            var result = await conn.QueryAsync<RevenueViewModel>("GetRevenueDaily", dynamicParameters, null, 120, CommandType.StoredProcedure);
            return Ok(result.ToList());
        }

        [HttpGet("count-sales-daily")]
        [ClaimRequirement(FunctionConstant.Revenue, CommandConstant.View)]
        public async Task<IActionResult> GetCountSalesDaily(int key, string dateFrom, string dateTo)
        {
            if (key == 1)
            {
                var now = DateTime.Now;
                var sevenDayAgo = now.Date.AddDays(-7);
                dateFrom = sevenDayAgo.ToString("yyyy/MM/dd");
                dateTo = now.ToString("yyyy/MM/dd");
            }
            else if (key == 2)
            {
                var now = DateTime.Now;
                var monthDayAgo = now.Date.AddDays(-30);
                dateFrom = monthDayAgo.ToString("yyyy/MM/dd");
                dateTo = now.ToString("yyyy/MM/dd");
            }

            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@fromDate", dateFrom);
            dynamicParameters.Add("@toDate", dateTo);
            var result = await conn.QueryAsync<RevenueViewModel>("GetCountSalesDaily", dynamicParameters, null, 120, CommandType.StoredProcedure);
            return Ok(result.ToList());
        }
    }
}