namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class FeedBackCreateRequest
    {
        public int Id { get; set; }

        public string Name { set; get; }

        public string Email { set; get; }

        public string PhoneNumber { get; set; }

        public string Message { set; get; }
    }
}