using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        public OrderDetail()
        {
        }

        public int OrderId { set; get; }

        public Guid ActiveCourseId { set; get; }

        public long? Price { set; get; }

        public long? PromotionPrice { set; get; }

        public Order Order { set; get; }

        public ActivateCourse ActivateCourse { set; get; }
    }
}