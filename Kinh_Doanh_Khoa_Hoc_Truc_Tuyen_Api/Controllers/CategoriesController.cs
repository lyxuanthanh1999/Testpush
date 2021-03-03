using Dapper;
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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly EKhoaHocDbContext _khoaHocDbContext;

        private ILogger<CategoriesController> _logger;

        private readonly IConfiguration _configuration;

        private readonly IStorageService _storageService;

        public CategoriesController(EKhoaHocDbContext khoaHocDbContext, ILogger<CategoriesController> logger, IStorageService storageService, IConfiguration configuration)
        {
            _khoaHocDbContext = khoaHocDbContext;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _khoaHocDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thầy danh mục với id: {id}"));
            }

            var category = new CategoryViewModel()
            {
                Name = result.Name,
                SortOrder = result.SortOrder,
                ParentId = result.ParentId,
                Id = result.Id
            };
            return Ok(category);
        }

        [HttpPost]
        [ClaimRequirement(FunctionConstant.Categories, CommandConstant.Create)]
        [ValidationFilter]
        public async Task<IActionResult> PostCategory([FromBody] CategoryCreateRequest request)
        {
            var category = new Category()
            {
                Name = request.Name,
                SortOrder = request.SortOrder,
                ParentId = request.ParentId
            };
            await _khoaHocDbContext.Categories.AddAsync(category);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = category.Id }, request);
            }

            return BadRequest(new ApiBadRequestResponse("Tạo danh mục thất bại"));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _khoaHocDbContext.Categories.Select(x => new CategoryViewModel()
            {
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId,
                Id = x.Id,
            }).ToListAsync());
        }

        [HttpGet("side-bar")]
        public IActionResult GetCategoriesBySideBar()
        {
            var dataParent = _khoaHocDbContext.Categories.AsNoTracking().Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder);
            var listCategories = new List<CategoryViewModel>();
            foreach (var category in dataParent)
            {
                var categoriesChild = _khoaHocDbContext.Categories.AsNoTracking().Where(x => x.ParentId == category.Id).OrderBy(x => x.SortOrder).Select(x => new CategoryViewModel
                {
                    Name = x.Name,
                    ParentId = x.ParentId,
                    SortOrder = x.SortOrder,
                    Id = x.Id
                }).ToList();
                var categoriesViewModel = new CategoryViewModel()
                {
                    Name = category.Name,
                    ParentId = category.ParentId,
                    SortOrder = category.SortOrder,
                    Id = category.Id,
                    CategoryViewModels = categoriesChild
                };
                listCategories.Add(categoriesViewModel);
            }

            return Ok(listCategories);
        }

        [HttpGet("home-categories")]
        public async Task<IActionResult> GetHomeCategories()
        {
            var dataParent = _khoaHocDbContext.Categories.AsNoTracking().Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder).Take(4);
            await using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            if (conn.State == ConnectionState.Closed)
            {
                await conn.OpenAsync();
            }

            //var now = DateTime.Now;
            //// lấy ra danh mục cha

            ////// lấy ra các sự kiện đang diễn ra => mục đích : lấy promotion price
            ////var lstPromotion = _khoaHocDbContext.Promotions.Include(x => x.PromotionInCourses).AsNoTracking()
            ////    .Where(x => x.FromDate <= now && now <= x.ToDate);
            var listCategories = new List<CategoryViewModel>();
            foreach (var item in dataParent)
            {
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@categoryId", item.Id);
                var courses = (await conn.QueryAsync<CourseViewModel>("HomeCategories", dynamicParameters, null, 120, CommandType.StoredProcedure)).ToList();
                // đưa dữ liệu vào viewModel (CategoriesViewModel là để thông tin danh mục cha là Ngoại Ngữ và danh mục con là Tiếng Anh)
                var categories = new CategoryViewModel()
                {
                    Name = item.Name, // dùng để chưa tên Danh mục cha là Ngoại Ngữ
                    SortOrder = item.SortOrder,
                    ParentId = item.ParentId,
                    Id = item.Id,
                    // CourseViewModels dùng để thông tin của khóa học theo danh mục con(Tiếng Anh)
                    CourseViewModels = courses.Select(c => new CourseViewModel()
                    {
                        Name = c.Name.formatData(20),
                        Status = c.Status,
                        CategoryId = c.CategoryId,
                        Price = c.Price,
                        CreationTime = c.CreationTime,
                        LastModificationTime = c.LastModificationTime,
                        Image = _storageService.GetFileUrl(c.Image),
                        Content = c.Content,
                        Description = c.Description,
                        Id = c.Id,
                        CategoryName = c.CategoryName.formatData(20),
                        DiscountAmount = c.DiscountAmount,
                        DiscountPercent = c.DiscountPercent
                    }).ToList(),
                };
                listCategories.Add(categories);
            }

            return Ok(listCategories);
        }

        [HttpGet("filter")]
        [ClaimRequirement(FunctionConstant.Categories, CommandConstant.View)]
        public async Task<IActionResult> GetCategoriesPaging(string filter, int pageIndex, int pageSize)
        {
            var query = _khoaHocDbContext.Categories.AsNoTracking().AsQueryable();
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter) || x.Name.Contains(filter));
            }

            var totalRecords = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new CategoryViewModel()
            {
                Name = x.Name,
                SortOrder = x.SortOrder,
                ParentId = x.ParentId,
                Id = x.Id,
            }).ToListAsync();
            var pagination = new Pagination<CategoryViewModel>
            {
                TotalRecords = totalRecords,
                Items = items,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            return Ok(pagination);
        }

        [HttpPut("{id}")]
        [ValidationFilter]
        [ClaimRequirement(FunctionConstant.Categories, CommandConstant.Update)]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CategoryCreateRequest request)
        {
            var category = await _khoaHocDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với id: {id}"));
            }
            if (id == request.ParentId)
            {
                return BadRequest(new ApiBadRequestResponse("Danh mục con không thể là con của danh mục cha này"));
            }
            category.Name = request.Name;
            category.ParentId = request.ParentId;
            category.SortOrder = request.SortOrder;
            _khoaHocDbContext.Categories.Update(category);
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest(new ApiBadRequestResponse("Cập nhật thất bại"));
        }

        [HttpGet("{functionId}/parents")]
        [ClaimRequirement(FunctionConstant.Categories, CommandConstant.View)]
        public async Task<IActionResult> GetFunctionsByParentId(int id)
        {
            var categories = _khoaHocDbContext.Categories.Where(x => x.Id != id).OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder).ThenBy(x => x.SortOrder);
            return Ok(await categories.Select(u => new CategoryViewModel
            {
                Id = u.Id,
                Name = u.Name,
                SortOrder = u.SortOrder,
                ParentId = u.ParentId,
            }).ToListAsync());
        }

        [HttpPost("delete-multi-items")]
        [ClaimRequirement(FunctionConstant.Categories, CommandConstant.Delete)]
        public async Task<IActionResult> DeleteCategory(List<int> ids)
        {
            foreach (var id in ids)
            {
                var category = await _khoaHocDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return NotFound(new ApiNotFoundResponse($"Không tìm thấy danh mục với id: {id}"));
                }
                _khoaHocDbContext.Categories.Remove(category);
            }
            var result = await _khoaHocDbContext.SaveChangesAsync();
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest(new ApiBadRequestResponse("Xóa danh mục thất bại"));
        }
    }
}