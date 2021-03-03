namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class AnnouncementToUserViewModel
    {
        public string UserId { get; set; } = "";

        public AnnouncementViewModel AnnouncementViewModel { get; set; } = new AnnouncementViewModel();
    }
}