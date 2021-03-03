namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class ResetPasswordRequest
    {
        public string UserId { get; set; }

        public string Password { get; set; }

        public string Token { get; set; }
    }
}