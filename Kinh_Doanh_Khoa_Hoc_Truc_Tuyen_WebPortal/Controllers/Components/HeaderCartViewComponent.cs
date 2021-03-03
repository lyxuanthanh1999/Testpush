using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.Common;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Extensions;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Services.Implements;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Controllers.Components
{
    public class HeaderCartViewComponent : ViewComponent
    {
        private readonly IBaseApiClient _apiClient;

        public HeaderCartViewComponent(IBaseApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(HttpContext.Session.Get<List<CartViewModel>>(SystemConstants.CartSession) ?? new List<CartViewModel>());
        }
    }
}