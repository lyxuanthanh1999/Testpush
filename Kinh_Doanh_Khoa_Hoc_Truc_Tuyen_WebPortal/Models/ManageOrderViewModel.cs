using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class ManageOrderViewModel
    {
        public Pagination<OrderViewModel> OrderViewModels { get; set; }

        public string SortType { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "all",Text = "Tất cả"},
            new SelectListItem(){Value = "15daysAgo",Text = "15 ngày trước"},
            new SelectListItem(){Value = "30daysAgo",Text = "30 ngày trước"},
            new SelectListItem(){Value = "6monthsAgo",Text = "6 tháng trước"},
            new SelectListItem(){Value = "1yearAgo",Text = "1 năm trước"}
        };

        public string NameDash { get; set; }
    }
}