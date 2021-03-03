using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IBaseApiClient _apiClient;

        public CoursesController(IBaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [HttpGet]
        [Route("courses.html")]
        public async Task<IActionResult> Index(int? categoryId, long? priceMin, long? priceMax, int? pageSize, string filter = null, string sortBy = null, int page = 1)
        {
            var courseCatalog = new CourseCatalogViewModel();
            pageSize ??= 3;
            courseCatalog.PageSize = pageSize;
            courseCatalog.SortType = sortBy;
            courseCatalog.PriceMax = priceMax;
            courseCatalog.PriceMin = priceMin;
            courseCatalog.Filter = filter;
            courseCatalog.CategoryViewModels = await _apiClient.GetListAsync<CategoryViewModel>($"/api/categories/side-bar");
            courseCatalog.Data = await _apiClient.GetAsync<Pagination<CourseViewModel>>($"/api/courses/client-filter?categoryId={categoryId}" + $"&pageIndex={page}" + $"&pageSize={pageSize}" + $"&priceMin={priceMin}" + $"&priceMax={priceMax}" + $"&sortBy={sortBy}" + $"&filter={filter}");
            if (User.Identity.IsAuthenticated)
            {
                courseCatalog.ActiveCoursesViewModels = await _apiClient.GetListAsync<ActiveCoursesViewModel>($"/api/courses/check-active-courses/user-{User.GetUserId()}");
            }
            return View(courseCatalog);
        }

        [HttpGet]
        [Route("courses-{id}.html")]
        public async Task<IActionResult> Detail(int id)
        {
            var detail = new CourseDetailClientViewModel();
            detail.CourseViewModel = await _apiClient.GetAsync<CourseViewModel>($"/api/courses/{id}");
            detail.LessonViewModels = await _apiClient.GetListAsync<LessonViewModel>($"/api/lessons/course-{id}");
            detail.CommentViewModels = await _apiClient.GetListAsync<CommentViewModel>($"/api/comments/courses/client?entityId={id}&entityType=courses");
            detail.RelatedCourses = await _apiClient.GetListAsync<CourseViewModel>($"/api/courses/related-courses/{id}");
            detail.UserViewModel = await _apiClient.GetAsync<UserViewModel>($"/api/users/course-{id}");
            if (User.Identity.IsAuthenticated)
            {
                detail.ActiveCoursesViewModels = await _apiClient.GetListAsync<ActiveCoursesViewModel>($"/api/courses/check-active-courses/user-{User.GetUserId()}");
            }
            return View(detail);
        }

        [HttpGet]
        [Route("lessons-with-courses-{id}.html")]
        public async Task<IActionResult> Lessons(int id, int? lessonId)
        {
            var data = new LessonsCatalogViewModel();
            data.CourseViewModel = await _apiClient.GetAsync<CourseViewModel>($"/api/courses/{id}");
            data.LessonViewModels = await _apiClient.GetListAsync<LessonViewModel>($"/api/lessons/course-{id}");
            data.LessonViewModel = await _apiClient.GetAsync<LessonViewModel>($"/api/lessons/course-{id}/detail?id={id}&lessonId={lessonId}");
            data.CommentViewModels = await _apiClient.GetListAsync<CommentViewModel>($"/api/comments/lessons/client?entityId={lessonId}&entityType=lessons");
            return View(data);
        }

        #region Ajax Method

        [HttpGet]
        public async Task<IActionResult> GetCoursesByFilter(string filter)
        {
            var data = await _apiClient.GetListAsync<string>($"/api/courses/filter-name");
            if (!string.IsNullOrEmpty(filter))
            {
                data = data.Where(x => x.ToLower().Contains(filter.ToLower())).ToList();
            }
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentById(int id, string entityType, int pageIndex = 1, int pageSize = 3)
        {
            var data = await _apiClient.GetAsync<Pagination<CommentViewModel>>(
                $"/api/comments/{entityType}/{id}/hierarchical?entityId={id}&entityType={entityType}&pageIndex={pageIndex}&pageSize={pageSize}");
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetRepliedCommentById(int id, string entityType, int rootCommentId, int pageIndex = 1, int pageSize = 3)
        {
            var data = await _apiClient.GetAsync<Pagination<CommentViewModel>>(
                $"/api/comments/{entityType}/{id}/root-{rootCommentId}/replied?entityId={id}&entityType={entityType}&pageIndex={pageIndex}&pageSize={pageSize}");
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewComment([FromForm] CommentCreateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Content))
                {
                    return BadRequest();
                }
                request.UserId = User.GetUserId();
                var result = await _apiClient.PostAsync<CommentCreateRequest, CommentViewModel>(
                    $"/api/comments/courses/{request.EntityId}", request);
                result.OwnerUser = User.GetFullName() + " (" + User.GetEmail() + ")";
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> EditComment([FromForm] CommentCreateRequest request)
        {
            try
            {
                request.UserId = User.GetUserId();
                await _apiClient.PutAsync(
                    $"/api/comments/{request.Id}/courses/{request.EntityId}", request);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                await _apiClient.Delete(
                    $"/api/comments/delete-single-comment?id={id}");
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #endregion Ajax Method
    }
}