using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    public sealed class AppUser : IdentityUser<Guid>, IDateTracking
    {
        public AppUser()
        {
        }

        public AppUser(Guid id, string userName, string email, string phoneNumber, DateTime dob)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Dob = dob;
        }

        public AppUser(Guid id, string userName, string email, string phoneNumber, DateTime dob, string biography)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Dob = dob;
            Biography = biography;
        }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public DateTime Dob { get; set; }

        public string Biography { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public List<Order> Orders { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Announcement> Announcements { get; set; }

        public List<ActivateCourse> ActivateCourses { get; set; }
    }
}