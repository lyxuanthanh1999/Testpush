using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize("Bearer")]
    public class FunctionsController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private readonly ILogger<FunctionsController> _logger;

        public FunctionsController(EKhoaHocDbContext khoaHocDbContext, ILogger<FunctionsController> logger)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        [HttpGet("{id}")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.View)]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _khoaHocDbContext.Functions.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                _logger.LogError($"Cannot found function with id {id}");
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy function với id {id}"));
            }

            return Ok(new FunctionViewModel()
            {
                Icon = result.Icon,
                Id = result.Id,
                Name = result.Name,
                ParentId = result.ParentId,
                SortOrder = result.SortOrder,
                Url = result.Url
            });
        }

        [HttpGet("{functionId}/parents")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.View)]
        public async Task<IActionResult> GetFunctionsByParentId(string functionId)
        {
            var functions = _khoaHocDbContext.Functions.Where(x => x.Id != functionId).OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).ThenBy(x => x.SortOrder);
            return Ok(await functions.Select(u => new FunctionViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Url = u.Url,
                SortOrder = u.SortOrder,
                ParentId = u.ParentId,
                Icon = u.Icon
            }).ToListAsync());
        }

        [HttpPost]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.Create)]
        [ValidationFilter]
        public async Task<IActionResult> PostFunction([FromBody] FunctionCreateRequest request)
        {
            var dbFunction = await _khoaHocDbContext.Functions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (dbFunction != null)
            {
                _logger.LogError($"Function with id {request.Id} is existed.");
                return BadRequest(new ApiBadRequestResponse($"Function này đã tồn tại với id {request.Id}"));
            }

            var function = new Function
            {
                Name = request.Name,
                ParentId = request.ParentId,
                Url = request.Url,
                Id = request.Id,
                SortOrder = request.SortOrder,
                Icon = request.Icon
            };
            await _khoaHocDbContext.Functions.AddAsync(function);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = function.Id }, request);
            }
            _logger.LogError("Create function is failed");
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpGet]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.View)]
        public async Task<IActionResult> GetFunctions()
        {
            return Ok(await _khoaHocDbContext.Functions.OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).Select(_ => new FunctionViewModel
            {
                Url = _.Url,
                SortOrder = _.SortOrder,
                ParentId = _.ParentId,
                Name = _.Name,
                Id = _.Id,
                Icon = _.Icon
            }).ToListAsync());
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.View)]
        public async Task<IActionResult> GetFunctionsPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Functions.OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(_ => _.Name.Contains(filter) || _.Id.Contains(filter) || _.Url.Contains(filter));
            }

            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageSize - 1) * pageSize).Take(pageSize).Select(_ => new FunctionViewModel
            {
                Name = _.Name,
                ParentId = _.ParentId,
                Url = _.Url,
                SortOrder = _.SortOrder,
                Id = _.Id,
                Icon = _.Icon
            }).ToListAsync();
            var pagination = new Pagination<FunctionViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpPut("{id}")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> PutFunction(string id, [FromBody] FunctionCreateRequest request)
        {
            var function = await _khoaHocDbContext.Functions.FirstOrDefaultAsync(x => x.Id == id);
            if (function == null)
            {
                _logger.LogError($"Cannot found function with id {id}");
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy function với id {id}"));
            }

            function.Name = request.Name;
            function.ParentId = request.ParentId;
            function.SortOrder = request.SortOrder;
            function.Url = request.Url;
            function.Icon = request.Icon;
            _khoaHocDbContext.Functions.Update(function);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            _logger.LogError("Update function failed");
            return BadRequest(new ApiBadRequestResponse("Update function failed"));
        }

        [HttpPost("delete-multi-items")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.Delete)]
        public async Task<IActionResult> DeleteFunction(List<string> ids)
        {
            foreach (var id in ids)
            {
                var function = await _khoaHocDbContext.Functions.FirstOrDefaultAsync(x => x.Id.Equals(id));
                if (function == null)
                {
                    _logger.LogError($"Cannot found function with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy function với id {id}"));
                }
                var commands = _khoaHocDbContext.CommandInFunctions.Where(x => x.FunctionId == id);
                if (commands.Any())
                {
                    _khoaHocDbContext.CommandInFunctions.RemoveRange(commands);
                }

                var permission = _khoaHocDbContext.Permissions.Where(x => x.FunctionId.Equals(id));
                if (permission.Any())
                {
                    _khoaHocDbContext.Permissions.RemoveRange(permission);
                }
                _khoaHocDbContext.Functions.Remove(function);
                var result = await _khoaHocDbContext.SaveChangesAsync();
                if (result <= 0)
                {
                    _logger.LogError("Delete function failed");
                    return BadRequest(new ApiBadRequestResponse("Delete thất bại"));
                }
            }
            return Ok();
        }

        [HttpGet("{functionId}/commands")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.View)]
        public async Task<IActionResult> GetCommandsInFunction(string functionId)
        {
            var query = from a in _khoaHocDbContext.Commands
                        join cif in _khoaHocDbContext.CommandInFunctions on a.Id equals cif.CommandId into result1
                        from commandInFunction in result1.DefaultIfEmpty()
                        join f in _khoaHocDbContext.Functions on commandInFunction.FunctionId equals f.Id into result2
                        from function in result2.DefaultIfEmpty()
                        select new
                        {
                            a.Id,
                            a.Name,
                            commandInFunction.FunctionId
                        };
            query = query.Where(x => x.FunctionId == functionId);
            return Ok(await query.Select(x => new CommandViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync());
        }

        [HttpPost("{functionId}/commands")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.Create)]
        [ValidationFilter]
        public async Task<IActionResult> PostCommandsToFunction(string functionId,
            [FromBody] CommandAssignRequest request)
        {
            foreach (var commandId in request.CommandIds)
            {
                var check = await _khoaHocDbContext.CommandInFunctions.FirstOrDefaultAsync(x => x.FunctionId.Equals(functionId) && x.CommandId.Equals(commandId));
                if (check != null)
                {
                    _logger.LogError("This command has been existed in function");
                    return BadRequest(new ApiBadRequestResponse("Command nãy đã tồn tại trong function này"));
                }
                var entity = new CommandInFunction()
                {
                    CommandId = commandId,
                    FunctionId = functionId
                };
                await _khoaHocDbContext.CommandInFunctions.AddAsync(entity);
            }
            if (request.AddToAllFunctions)
            {
                var otherFunctions = _khoaHocDbContext.Functions.Where(x => x.Id != functionId);
                foreach (var function in otherFunctions)
                {
                    foreach (var commandId in request.CommandIds)
                    {
                        if (await _khoaHocDbContext.CommandInFunctions.FirstOrDefaultAsync(x => x.FunctionId.Equals(function.Id) && x.CommandId.Equals(commandId)) == null)
                        {
                            await _khoaHocDbContext.CommandInFunctions.AddAsync(new CommandInFunction()
                            {
                                CommandId = commandId,
                                FunctionId = function.Id
                            });
                        }
                    }
                }
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Add command to function failed");
            return BadRequest(new ApiBadRequestResponse("Thêm command vào function thất bại"));
        }

        [HttpPost("{functionId}/commands/delete-items")]
        [ClaimRequirement(FunctionConstant.Function, CommandConstant.Delete)]
        public async Task<IActionResult> DeleteCommandToFunction(string functionId, CommandAssignRequest request)
        {
            foreach (var commandId in request.CommandIds)
            {
                var entity = await _khoaHocDbContext.CommandInFunctions.FirstOrDefaultAsync(x => x.FunctionId.Equals(functionId) && x.CommandId.Equals(commandId));
                if (entity == null)
                    return BadRequest(new ApiBadRequestResponse("Command này không có trong function này"));
                _khoaHocDbContext.CommandInFunctions.Remove(entity);
            }

            var result = await _khoaHocDbContext.SaveChangesAsync();

            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Delete command to function failed");
            return BadRequest(new ApiBadRequestResponse("Xóa command trong function thất bại"));
        }
    }
}