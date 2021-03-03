using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => new { x.CommandId, x.FunctionId, x.RoleId });
            builder.HasOne(x => x.Command).WithMany(x => x.Permissions).HasForeignKey(x => x.CommandId);
            builder.HasOne(x => x.AppRole).WithMany(x => x.Permissions).HasForeignKey(x => x.RoleId);
            builder.HasOne(x => x.Function).WithMany(x => x.Permissions).HasForeignKey(x => x.FunctionId);
        }
    }
}