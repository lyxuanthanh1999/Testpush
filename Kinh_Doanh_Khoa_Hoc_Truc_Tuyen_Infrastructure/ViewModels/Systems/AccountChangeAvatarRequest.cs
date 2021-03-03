using Microsoft.AspNetCore.Http;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class AccountChangeAvatarRequest
    {
        public string Id { get; set; }

        public IFormFile Avatar { get; set; }
    }
}