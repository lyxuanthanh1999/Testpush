using System.ComponentModel;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Domain.Enums
{
    public enum PaymentMethod
    {
        // Trả hàng lúc nhận tiền
        [Description("Cash On Delivery")]
        CashOnDelivery,

        // Trả tiền qua Ví điện tử
        [Description("Payment Gateway")]
        PaymentGateway
    }
}