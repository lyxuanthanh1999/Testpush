using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("CommandInFunctions")]
    public class CommandInFunction
    {
        public string CommandId { get; set; }

        public string FunctionId { get; set; }

        public Command Command { get; set; }

        public Function Function { get; set; }
    }
}