using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Content).IsRequired();
            builder.Property(x => x.Title).IsRequired();
            builder.HasMany(x => x.AnnouncementUsers).WithOne(x => x.Announcement).HasForeignKey(x => x.AnnouncementId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Announcements).HasForeignKey(x => x.UserId);
        }
    }
}