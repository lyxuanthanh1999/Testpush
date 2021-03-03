using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class CartViewModel
    {
        public CourseViewModel CourseViewModel { get; set; }

        public long Price { get; set; }

        public long? PromotionPrice { get; set; }
    }
}