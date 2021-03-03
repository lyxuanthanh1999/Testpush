using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class MyCoursesViewModel
    {
        public Pagination<CourseViewModel> CourseViewModels { get; set; }

        public string NameDash { get; set; }

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "3",Text = "3"},
            new SelectListItem(){Value = "5",Text = "5"},
            new SelectListItem(){Value = "10",Text = "10"},
        };

        public int? PageSize { get; set; }
    }
}