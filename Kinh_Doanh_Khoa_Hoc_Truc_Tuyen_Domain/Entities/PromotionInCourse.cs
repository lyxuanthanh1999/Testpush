using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("PromotionInCourses")]
    public class PromotionInCourse
    {
        public int PromotionId { get; set; }

        public int CourseId { get; set; }

        public Promotion Promotion { get; set; }

        public Course Course { get; set; }
    }
}