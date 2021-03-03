using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Promotions")]
    public class Promotion : ISwitchable
    {
        public int Id { set; get; }

        public DateTime FromDate { set; get; }

        public DateTime ToDate { set; get; }

        public bool ApplyForAll { set; get; }

        public int? DiscountPercent { set; get; }

        public long? DiscountAmount { set; get; }

        public bool Status { set; get; }

        public string Name { set; get; }

        public string Content { get; set; }

        public List<PromotionInCourse> PromotionInCourses { get; set; }
    }
}