using System.ComponentModel.DataAnnotations;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Scope { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public bool RememberMe { get; set; }
    }
}