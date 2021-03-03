namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class UserPasswordChangeRequest
    {
        public string Id { get; set; }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}