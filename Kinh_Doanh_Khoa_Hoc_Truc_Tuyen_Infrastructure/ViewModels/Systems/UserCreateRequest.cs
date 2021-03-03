using Microsoft.AspNetCore.Http;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class UserCreateRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string Dob { get; set; }

        public string Biography { get; set; }

        public IFormFile Avatar { get; set; }
    }
}