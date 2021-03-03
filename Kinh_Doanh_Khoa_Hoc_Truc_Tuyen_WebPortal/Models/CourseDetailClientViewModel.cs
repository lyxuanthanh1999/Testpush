using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class CourseDetailClientViewModel
    {
        public CourseViewModel CourseViewModel { get; set; }

        public UserViewModel UserViewModel { get; set; }

        public List<LessonViewModel> LessonViewModels { get; set; }

        public List<CommentViewModel> CommentViewModels { get; set; }

        public List<CourseViewModel> RelatedCourses { get; set; }

        public List<ActiveCoursesViewModel> ActiveCoursesViewModels { get; set; }
    }
}