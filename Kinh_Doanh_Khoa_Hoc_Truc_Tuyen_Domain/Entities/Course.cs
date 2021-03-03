using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Courses")]
    public class Course : IDateTracking
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Image { get; set; }

        public string Content { get; set; }

        public long Price { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int Status { get; set; }

        public int CategoryId { get; set; }

        public string CreatedUserName { get; set; }

        public Category Category { get; set; }

        public List<Lesson> Lessons { get; set; }

        public List<ActivateCourse> ActivateCourses { get; set; }

        public List<PromotionInCourse> PromotionInCourses { get; set; }
    }
}