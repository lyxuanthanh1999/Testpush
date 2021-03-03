using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Announcements")]
    public class Announcement : IDateTracking
    {
        public Announcement()
        {
        }

        public Announcement(Guid id, string title, string content, string image, Guid? userId, int status)
        {
            Id = id;
            Title = title;
            Content = content;
            Image = image;
            UserId = userId;
            Status = status;
        }

        public Guid Id { get; set; }

        public string Title { set; get; }

        public string Content { set; get; }

        public string Image { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public Guid? UserId { set; get; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public int Status { set; get; }

        public AppUser AppUser { get; set; }

        public List<AnnouncementUser> AnnouncementUsers { get; set; }
    }
}