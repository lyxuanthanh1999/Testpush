using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class PermissionViewModel
    {
        public string FunctionId { get; set; }

        public Guid RoleId { get; set; }

        public string CommandId { get; set; }
    }
}