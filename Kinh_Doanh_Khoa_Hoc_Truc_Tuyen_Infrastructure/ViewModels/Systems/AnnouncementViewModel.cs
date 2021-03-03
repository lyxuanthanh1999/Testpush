using System;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class AnnouncementViewModel
    {
        public Guid Id { get; set; }

        public string Title { set; get; }

        public string Content { set; get; }

        public string Image { get; set; }

        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public Guid? UserId { set; get; }

        public string UserFullName { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public bool TmpHasRead { get; set; }

        public int Status { set; get; }

        public List<AnnouncementUserViewModel> AnnouncementUserViewModels { get; set; }
    }
}