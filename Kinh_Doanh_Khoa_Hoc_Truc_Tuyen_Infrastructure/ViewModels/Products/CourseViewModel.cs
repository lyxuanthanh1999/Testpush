using System;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }

        public long Price { get; set; }

        public int? DiscountPercent { set; get; }

        public long? DiscountAmount { set; get; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int Status { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public long? SortPrice { get; set; }

        public string CreatedUserName { get; set; }

        public string CreatedName { get; set; }

        public int CountStudent { get; set; }

        public List<CommentViewModel> CommentViewModels { get; set; }

        public List<LessonViewModel> LessonViewModels { get; set; }
    }
}