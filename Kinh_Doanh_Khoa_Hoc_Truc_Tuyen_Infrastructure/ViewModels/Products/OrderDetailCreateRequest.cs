using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class OrderDetailCreateRequest
    {
        public int OrderId { set; get; }

        public Guid ActiveCourseId { set; get; }

        public long? Price { set; get; }

        public long? PromotionPrice { set; get; }

        public string CourseName { get; set; }
    }
}