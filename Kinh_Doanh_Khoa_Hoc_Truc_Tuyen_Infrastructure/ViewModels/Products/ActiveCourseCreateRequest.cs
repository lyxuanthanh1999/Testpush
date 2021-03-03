namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class ActiveCourseCreateRequest
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public int CourseId { get; set; }

        public bool Status { get; set; }
    }
}