using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;
using System.Collections.Generic;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class CommentViewModelResponse
    {
        public List<CommentViewModel> CommentViewModels { get; set; }

        public int TotalRecord { get; set; }
    }
}