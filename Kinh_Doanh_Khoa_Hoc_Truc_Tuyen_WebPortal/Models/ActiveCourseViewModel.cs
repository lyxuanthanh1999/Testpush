using System.ComponentModel.DataAnnotations;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_WebPortal.Models
{
    public class ActiveCourseViewModel
    {
        [Required(ErrorMessage = "Yêu Cầu Nhập Mã Kích Hoạt", AllowEmptyStrings = false)]
        public string Code { get; set; }

        public string NameDash { get; set; }
    }
}