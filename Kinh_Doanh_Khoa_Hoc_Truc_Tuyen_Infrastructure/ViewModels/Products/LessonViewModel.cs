namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class LessonViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string VideoPath { get; set; }

        public string Attachment { get; set; }

        public int SortOrder { get; set; }

        public int Status { get; set; }

        public int CourseId { get; set; }

        public string Times { get; set; }

        public string CourseName { get; set; }

        public string TotalTimes { get; set; }
    }
}