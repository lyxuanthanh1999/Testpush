using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class MyAnnouncementViewModel
    {
        public Pagination<AnnouncementViewModel> AnnouncementViewModels { get; set; }

        public string FilterType { get; set; }

        public int TmpPage { get; set; }

        public List<SelectListItem> FilterTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem(){Value = "true",Text = "Tất cả"},
            new SelectListItem(){Value = "false",Text = "Thông báo chưa đọc"},
        };

        public string NameDash { get; set; }
    }
}