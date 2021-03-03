using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequest>
    {
        public UserCreateRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Yêu cầu nhập tên đăng nhập");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Yêu cầu nhập mật khẩu").MinimumLength(4)
                .WithMessage("Mật khẩu ít nhất 4 ký tự");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Yêu cầu nhập email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Định dạng email không chính xác");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Yêu cầu nhập số điện thoại");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Yêu cầu nhập tên")
                .MaximumLength(50).WithMessage("Họ và tên phải dưới 50 ký tự");
        }
    }
}