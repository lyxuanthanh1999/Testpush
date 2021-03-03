using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Price).HasDefaultValue(0);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.Image).IsRequired();
            builder.HasOne(x => x.Category).WithMany(x => x.Courses).HasForeignKey(x => x.CategoryId);
            builder.HasMany(x => x.Lessons).WithOne(x => x.Course).HasForeignKey(x => x.CourseId);
        }
    }
}