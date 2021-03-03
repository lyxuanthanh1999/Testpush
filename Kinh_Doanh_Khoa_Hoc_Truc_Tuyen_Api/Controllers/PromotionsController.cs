using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Claims;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Filter;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Helpers;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Services;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.EF;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
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
    public class PromotionsController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<PromotionsController> _logger;

        private readonly IStorageService _storageService;

        public PromotionsController(EKhoaHocDbContext khoaHocDbContext, ILogger<PromotionsController> logger, IStorageService storageService)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger;
            _storageService = storageService;
        }

        [HttpGet("{id}")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.View)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                _logger.LogError($"Cannot found Course with id {id}");
                return NotFound(new ApiNotFoundResponse($"Không thể tìm thấy khóa học với id {id}"));
            }

            return Ok(new PromotionViewModel()
            {
                Name = result.Name,
                Status = result.Status,
                ToDate = result.ToDate,
                DiscountPercent = result.DiscountPercent,
                FromDate = result.FromDate,
                DiscountAmount = result.DiscountAmount,
                ApplyForAll = result.ApplyForAll,
                Id = result.Id,
                Content = result.Content
            });
        }

        [HttpPost]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Create)]
        public async Task<IActionResult> PostPromotion(PromotionCreateRequest request)
        {
            if (DateTime.Compare(DateTime.Parse(request.FromDate).Date, DateTime.Parse(request.ToDate).Date) > 0)
                return BadRequest(new ApiBadRequestResponse("Ngày kết thúc phải lớn hơn ngày bắt đầu"));
            var dbPromotion = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (dbPromotion != null)
            {
                _logger.LogError($"Course with id {request.Id} is existed.");
                return BadRequest(new ApiBadRequestResponse($"Khóa học này đã tồn tại với id {request.Id}"));
            }
            var promotion = new Promotion()
            {
                Name = request.Name,
                Status = request.Status,
                FromDate = DateTime.Parse(request.FromDate),
                ToDate = DateTime.Parse(request.ToDate),
                ApplyForAll = false,
                DiscountAmount = request.DiscountAmount,
                DiscountPercent = request.DiscountPercent,
                Content = request.Content
            };
            await _khoaHocDbContext.Promotions.AddAsync(promotion);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = promotion.Id }, request);
            }
            _logger.LogError("Create course is failed");
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.View)]
        public IActionResult GetPromotionsPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Promotions.AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.Name.ToLower().Contains(filter.ToLower()) || x.Name.convertToUnSign().ToLower()
                        .Contains(filter.convertToUnSign().ToLower()) || x.Content.ToLower().Contains(filter.ToLower()) || x.Content.convertToUnSign().ToLower()
                        .Contains(filter.convertToUnSign().ToLower()));
            }

            var data = query.ToList();
            var totalRecords = data.Count();
            var items = data.OrderBy(x => x.Status).ThenBy(x => x.ToDate).Skip((pageIndex - 1) * pageSize)
                .Take(pageSize).Select(_ => new PromotionCreateRequest()
                {
                    Id = _.Id,
                    Name = _.Name,
                    Status = _.Status,
                    FromDate = _.FromDate.ToString("dd/MM/yyyy"),
                    ToDate = _.ToDate.ToString("dd/MM/yyyy"),
                    ApplyForAll = false,
                    DiscountAmount = (int?)_.DiscountAmount,
                    DiscountPercent = _.DiscountPercent,
                    Content = _.Content
                }).ToList();
            var pagination = new Pagination<PromotionCreateRequest>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpPut("{id}")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        [ValidationFilter]
        public async Task<IActionResult> PutPromotion(PromotionCreateRequest request)
        {
            if (DateTime.Compare(DateTime.Parse(request.FromDate).Date, DateTime.Parse(request.ToDate).Date) > 0)
                return BadRequest(new ApiBadRequestResponse("Ngày kết thúc phải lớn hơn ngày bắt đầu"));
            var promotion = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (promotion == null)
            {
                _logger.LogError($"Cannot found promotion with id {request.Id}");
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy sự kiện với id {request.Id}"));
            }

            promotion.Name = request.Name;
            promotion.Status = request.Status;
            promotion.FromDate = DateTime.Parse(request.FromDate);
            promotion.ToDate = DateTime.Parse(request.ToDate);
            promotion.DiscountAmount = request.DiscountAmount;
            promotion.DiscountPercent = request.DiscountPercent;
            promotion.Content = request.Content;
            _khoaHocDbContext.Promotions.Update(promotion);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            _logger.LogError("Update promotion failed");
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpPost("delete-multi-items")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Delete)]
        public async Task<IActionResult> DeletePromotion(List<int> input)
        {
            foreach (var id in input)
            {
                var promotion = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == id);
                if (promotion == null)
                {
                    _logger.LogError($"Cannot found promotion with id {id}");
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy sự kiện với id {id}"));
                }
                var promotionInCourses = _khoaHocDbContext.PromotionInCourses.Where(x => x.PromotionId == id);
                if (promotionInCourses.Any())
                {
                    _khoaHocDbContext.PromotionInCourses.RemoveRange(promotionInCourses);
                }
                _khoaHocDbContext.Promotions.Remove(promotion);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            _logger.LogError("Delete Course failed");
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }

        [HttpGet("{id}/courses")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        public async Task<IActionResult> GetPromotionCourses(int id)
        {
            var promotion = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == id);
            if (promotion == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy sự kiện với id {id}"));
            var promotionCourses = _khoaHocDbContext.PromotionInCourses.Include(x => x.Course)
                .Where(x => x.PromotionId == promotion.Id && x.Course.Status != 3).Select(x => new CourseViewModel()
                {
                    Id = x.CourseId,
                    Name = x.Course.Name,
                    Image = _storageService.GetFileUrl(x.Course.Image),
                    CategoryId = x.Course.CategoryId,
                    Content = x.Course.Content,
                    Description = x.Course.Description,
                    Price = x.Course.Price,
                    Status = x.Course.Status,
                    CategoryName = x.Course.Category.Name,
                });
            return Ok(promotionCourses);
        }

        [HttpPost("{promotionId}/courses")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        public async Task<IActionResult> PostPromotionInCourses(int promotionId, List<int> request)
        {
            var promotion = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == promotionId);
            if (promotion == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy sự kiện với id {promotionId}"));
            var existingPromotion = _khoaHocDbContext.Promotions.Include(x => x.PromotionInCourses)
                .Where(x => x.ToDate >= promotion.FromDate && x.Id != promotionId).ToList();
            foreach (var promotionCourses in request.Select(id => new PromotionInCourse() { CourseId = id, PromotionId = promotionId }))
            {
                if (existingPromotion.Any(x => x.PromotionInCourses.Any(pic => pic.CourseId == promotionCourses.CourseId)))
                {
                    var detail = await _khoaHocDbContext.Courses.FirstOrDefaultAsync(x => x.Id == promotionCourses.CourseId);
                    return BadRequest(new ApiBadRequestResponse(
                        $"Không thể tạo khóa học (Id: {detail.Id} - Tên: {detail.Name}) trong sự kiện với id: {promotion.Id} vì khóa học này đã tồn tại trong sự kiện khác"));
                }
                await _khoaHocDbContext.PromotionInCourses.AddAsync(promotionCourses);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest(new ApiBadRequestResponse("Tạo thất bại"));
        }

        [HttpGet("filter/courses")]
        [ClaimRequirement(FunctionConstant.Courses, CommandConstant.View)]
        public IActionResult GetCoursesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Courses.Include(x => x.Category).Where(x => x.Status != 3).AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x =>
                    x.Name.ToLower().Contains(filter.ToLower()) || x.Name.convertToUnSign().ToLower()
                        .Contains(filter.convertToUnSign().ToLower()));
            }

            var data = query.ToList();
            var totalRecords = data.Count();
            var items = data.OrderBy(x => x.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(_ => new CourseViewModel
            {
                Id = _.Id,
                Image = _storageService.GetFileUrl(_.Image),
                Name = _.Name,
                CategoryId = _.CategoryId,
                Status = _.Status,
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

        [HttpPost("{promotionId}/courses/delete-multi-items")]
        [ClaimRequirement(FunctionConstant.Promotions, CommandConstant.Update)]
        public async Task<IActionResult> RemovePromotionInCourses(int promotionId, List<int> request)
        {
            var promotion = await _khoaHocDbContext.Promotions.FirstOrDefaultAsync(x => x.Id == promotionId);
            if (promotion == null)
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy sự kiện với id {promotionId}"));
            foreach (var id in request)
            {
                var promotionInCourses =
                    await _khoaHocDbContext.PromotionInCourses.FirstOrDefaultAsync(x =>
                        x.PromotionId == promotionId && x.CourseId == id);
                _khoaHocDbContext.PromotionInCourses.Remove(promotionInCourses);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
                return Ok();
            return BadRequest(new ApiBadRequestResponse("Xóa thất bại"));
        }
    }
}