using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Commands")]
    public class Command
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<CommandInFunction> CommandInFunctions { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}