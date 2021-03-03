namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems
{
    public class FunctionCreateRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int SortOrder { get; set; }

        public string ParentId { get; set; }

        public string Icon { get; set; }
    }
}