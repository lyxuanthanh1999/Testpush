using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Entities
{
    [Table("Permissions")]
    public class Permission
    {
        public Permission(string functionId, Guid roleId, string commandId)
        {
            FunctionId = functionId;
            RoleId = roleId;
            CommandId = commandId;
        }

        public string FunctionId { get; set; }

        public Guid RoleId { get; set; }

        public string CommandId { get; set; }

        public Function Function { get; set; }

        public AppRole AppRole { get; set; }

        public Command Command { get; set; }
    }
}