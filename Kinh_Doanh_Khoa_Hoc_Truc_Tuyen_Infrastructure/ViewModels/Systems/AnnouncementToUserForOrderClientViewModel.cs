using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class AnnouncementToUserForOrderClientViewModel
    {
        public AnnouncementViewModel AnnouncementViewModel { get; set; } = new AnnouncementViewModel();

        public OrderViewModel OrderViewModel { get; set; } = new OrderViewModel();
    }
}