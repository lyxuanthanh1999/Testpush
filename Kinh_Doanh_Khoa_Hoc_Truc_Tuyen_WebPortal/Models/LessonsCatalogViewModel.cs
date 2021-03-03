using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class LessonsCatalogViewModel
    {
        public CourseViewModel CourseViewModel { get; set; }

        public LessonViewModel LessonViewModel { get; set; }

        public List<LessonViewModel> LessonViewModels { get; set; }

        public List<CommentViewModel> CommentViewModels { get; set; }
    }
}