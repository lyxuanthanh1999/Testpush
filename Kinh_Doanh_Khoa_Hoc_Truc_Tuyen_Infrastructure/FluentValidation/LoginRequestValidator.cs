using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Yêu cầu nhập tên đăng nhập");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Yêu cầu nhập mật khẩu")
                .MinimumLength(6).WithMessage("Mật khẩu phải ít nhất 6 ký tự");
        }
    }
}