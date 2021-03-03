using System.ComponentModel;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Enums
{
    public enum OrderStatus
    {
        [Description("New Order")]
        New,

        [Description("In Progress")]
        InProgress,

        [Description("Returned")]
        Returned,

        [Description("Cancelled")]
        Cancelled,

        [Description("Completed")]
        Completed
    }
}