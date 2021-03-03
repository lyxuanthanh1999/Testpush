using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("AnnouncementUsers")]
    public class AnnouncementUser
    {
        public AnnouncementUser()
        {
        }

        public AnnouncementUser(int id, Guid announcementId, Guid userId, bool hasRead)
        {
            Id = id;
            AnnouncementId = announcementId;
            UserId = userId;
            HasRead = hasRead;
        }

        public int Id { get; set; }

        public Guid UserId { get; set; }

        public bool HasRead { get; set; }

        public Guid AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }
    }
}