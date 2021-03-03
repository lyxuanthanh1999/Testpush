using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> HomeCategoryViewModels { get; set; }

        public List<CourseViewModel> NewCourses { get; set; }

        public List<ActiveCoursesViewModel> ActiveCoursesViewModels { get; set; }
    }
}