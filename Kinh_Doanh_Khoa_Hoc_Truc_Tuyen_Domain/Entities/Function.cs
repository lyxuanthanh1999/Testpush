using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Functions")]
    public class Function : ISortable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int SortOrder { get; set; }

        public string ParentId { get; set; }

        public string Icon { get; set; }

        public List<CommandInFunction> CommandInFunctions { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}