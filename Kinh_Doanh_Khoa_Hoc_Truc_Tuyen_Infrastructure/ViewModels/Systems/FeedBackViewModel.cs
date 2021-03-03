using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class FeedBackViewModel
    {
        public int Id { get; set; }

        public string Name { set; get; }

        public string Email { set; get; }

        public string PhoneNumber { get; set; }

        public string Message { set; get; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}