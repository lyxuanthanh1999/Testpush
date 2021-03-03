namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class AnnouncementCreateRequest
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string UserId { get; set; }

        public int Status { get; set; }
    }
}