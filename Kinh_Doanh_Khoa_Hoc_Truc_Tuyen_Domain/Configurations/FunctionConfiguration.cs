using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class FunctionConfiguration : IEntityTypeConfiguration<Function>
    {
        public void Configure(EntityTypeBuilder<Function> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Url).IsRequired();
            builder.Property(x => x.Icon).HasColumnType("varchar(100)").HasMaxLength(100);
        }
    }
}