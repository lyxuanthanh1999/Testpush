using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class UpdatePermissionRequest
    {
        public List<PermissionViewModel> Permissions { get; set; } = new List<PermissionViewModel>();
    }
}