namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products
{
    public class CategoryCreateRequest
    {
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int? ParentId { get; set; }
    }
}