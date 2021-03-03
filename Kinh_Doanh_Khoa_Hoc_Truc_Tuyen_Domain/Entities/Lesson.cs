using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Lessons")]
    public class Lesson : ISortable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string VideoPath { get; set; }

        public string Attachment { get; set; }

        public int SortOrder { get; set; }

        public int Status { get; set; }

        public int CourseId { get; set; }

        public string Times { get; set; }

        public Course Course { get; set; }
    }
}