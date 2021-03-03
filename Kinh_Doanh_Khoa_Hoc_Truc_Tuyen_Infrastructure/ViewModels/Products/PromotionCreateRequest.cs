namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class PromotionCreateRequest
    {
        public int Id { get; set; }

        public string FromDate { set; get; }

        public string ToDate { set; get; }

        public bool ApplyForAll { set; get; }

        public int? DiscountPercent { set; get; }

        public long? DiscountAmount { set; get; }

        public bool Status { set; get; }

        public string Name { set; get; }

        public string Content { get; set; }
    }
}