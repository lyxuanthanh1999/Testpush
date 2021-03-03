using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class CourseCatalogViewModel
    {
        public Pagination<CourseViewModel> Data { get; set; }

        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public long? PriceMin { get; set; }

        public long? PriceMax { get; set; }

        public string Filter { get; set; }

        public string SortType { get; set; }

        public int? PageSize { get; set; }

        public List<ActiveCoursesViewModel> ActiveCoursesViewModels { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "lastest",Text = "Sắp Xếp: Mới Nhất"},
            new SelectListItem(){Value = "price_low_to_high",Text = "Sắp Xếp: Giá Tăng Dần"},
            new SelectListItem(){Value = "price_high_to_low",Text = "Sắp Xếp: Giá Giảm Dần"},
            new SelectListItem(){Value = "name",Text = "Sắp Xếp: Name"},
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "3",Text = "Hiển Thị: 3"},
            new SelectListItem(){Value = "6",Text = "Hiển Thị: 6"},
            new SelectListItem(){Value = "9",Text = "Hiển Thị: 9"},
        };
    }
}