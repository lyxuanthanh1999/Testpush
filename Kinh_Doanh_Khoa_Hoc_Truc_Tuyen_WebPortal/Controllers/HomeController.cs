using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseApiClient _apiClient;

        public HomeController(IBaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.NewCourses = await _apiClient.GetListAsync<CourseViewModel>($"/api/courses/new-courses");
            homeViewModel.HomeCategoryViewModels = await _apiClient.GetListAsync<CategoryViewModel>($"/api/categories/home-categories");
            if (User.Identity.IsAuthenticated)
            {
                homeViewModel.ActiveCoursesViewModels = await _apiClient.GetListAsync<ActiveCoursesViewModel>($"/api/courses/check-active-courses/user-{User.GetUserId()}");
            }
            return View(homeViewModel);
        }

        public IActionResult RefreshCart()
        {
            return ViewComponent("HeaderCart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}