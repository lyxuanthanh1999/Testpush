using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class AnnouncementUserViewModel
    {
        public int Id { get; set; }

        public Guid? UserId { get; set; }

        public bool HasRead { get; set; }

        public Guid AnnouncementId { get; set; }
    }
}