using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class ActivateCourseConfiguration : IEntityTypeConfiguration<ActivateCourse>
    {
        public void Configure(EntityTypeBuilder<ActivateCourse> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.AppUser).WithMany(x => x.ActivateCourses).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Course).WithMany(x => x.ActivateCourses).HasForeignKey(x => x.CourseId);
        }
    }
}