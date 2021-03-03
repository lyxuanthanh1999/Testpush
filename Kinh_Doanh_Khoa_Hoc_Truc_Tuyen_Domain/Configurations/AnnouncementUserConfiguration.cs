using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Configurations
{
    public class AnnouncementUserConfiguration : IEntityTypeConfiguration<AnnouncementUser>
    {
        public void Configure(EntityTypeBuilder<AnnouncementUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}