using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    public sealed class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
        }

        public AppRole(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public List<Permission> Permissions { get; set; }
    }
}