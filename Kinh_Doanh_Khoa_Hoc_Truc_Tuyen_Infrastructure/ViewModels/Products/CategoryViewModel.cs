using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public List<CategoryViewModel> CategoryViewModels { get; set; }

        public List<CourseViewModel> CourseViewModels { get; set; }
    }
}