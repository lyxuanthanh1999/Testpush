using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class PromotionInCourseConfiguration : IEntityTypeConfiguration<PromotionInCourse>
    {
        public void Configure(EntityTypeBuilder<PromotionInCourse> builder)
        {
            builder.HasKey(x => new { x.PromotionId, x.CourseId });
            builder.HasOne(x => x.Course).WithMany(x => x.PromotionInCourses).HasForeignKey(x => x.CourseId);
            builder.HasOne(x => x.Promotion).WithMany(x => x.PromotionInCourses).HasForeignKey(x => x.PromotionId);
        }
    }
}