using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Categories")]
    public class Category : ISortable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int? ParentId { get; set; }

        public List<Course> Courses { get; set; }
    }
}