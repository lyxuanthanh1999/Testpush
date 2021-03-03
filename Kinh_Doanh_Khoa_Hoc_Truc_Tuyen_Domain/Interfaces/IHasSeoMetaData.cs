namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces
{
    public interface IHasSeoMetaData
    {
        public string SeoTitle { get; set; }

        public string SeoAlias { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }
    }
}