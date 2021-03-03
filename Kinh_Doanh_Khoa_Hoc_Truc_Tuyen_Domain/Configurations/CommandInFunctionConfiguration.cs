using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class CommandInFunctionConfiguration : IEntityTypeConfiguration<CommandInFunction>
    {
        public void Configure(EntityTypeBuilder<CommandInFunction> builder)
        {
            builder.HasKey(c => new { c.CommandId, c.FunctionId });
            builder.HasOne(x => x.Function).WithMany(x => x.CommandInFunctions).HasForeignKey(x => x.FunctionId);
            builder.HasOne(x => x.Command).WithMany(x => x.CommandInFunctions).HasForeignKey(x => x.CommandId);
        }
    }
}