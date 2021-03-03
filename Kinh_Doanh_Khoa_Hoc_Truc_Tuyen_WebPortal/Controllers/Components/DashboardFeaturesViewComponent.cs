using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Enums;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers.Components
{
    public class DashboardFeaturesViewComponent : ViewComponent
    {
        private readonly IBaseApiClient _apiClient;

        public DashboardFeaturesViewComponent(IBaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name)
        {
            var data = new DashboardFeaturesViewModel();
            var userId = ((ClaimsIdentity)User.Identity)?.Claims
                .SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var fullName = ((ClaimsIdentity)User.Identity)?
                .Claims
                .SingleOrDefault(x => x.Type == "FullName")
                ?.Value;
            var items = await _apiClient.GetListAsync<OrderViewModel>($"/api/orders/user-{userId}");
            data.CountNewOrder = items.Count(x => x.Status == OrderStatus.New || x.Status == OrderStatus.InProgress);
            data.CountCanceledOrder = items.Count(x => x.Status == OrderStatus.Cancelled || x.Status == OrderStatus.Returned);
            data.CountOrderPlaced = items.Count(x => x.Status == OrderStatus.Completed);
            data.NameDashBoard = name;
            data.FullName = fullName;
            return View(data);
        }
    }
}