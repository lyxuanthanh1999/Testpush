using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IBaseApiClient _apiClient;

        public SideBarViewComponent(IBaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _apiClient.GetListAsync<CategoryViewModel>($"/api/categories/side-bar");
            return View(items);
        }
    }
}