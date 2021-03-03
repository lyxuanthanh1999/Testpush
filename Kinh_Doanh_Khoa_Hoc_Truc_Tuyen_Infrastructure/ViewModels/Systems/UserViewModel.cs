using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public DateTime Dob { get; set; }

        public string Biography { get; set; }

        public string Avatar { get; set; }

        public int CountStudents { get; set; }

        public int CountCourses { get; set; }

        public bool ConfirmEmail { get; set; }
    }
}