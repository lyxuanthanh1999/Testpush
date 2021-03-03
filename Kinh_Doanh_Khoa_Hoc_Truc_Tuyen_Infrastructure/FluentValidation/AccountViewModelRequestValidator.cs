using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class AccountViewModelRequestValidator : AbstractValidator<AccountViewModel>
    {
        public AccountViewModelRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Yêu cầu nhập tên đăng nhập");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Yêu cầu nhập email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Định dạng email không đúng");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Yêu cầu nhập số điện thoại");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Yêu cầu nhập họ và tên")
                .MaximumLength(50).WithMessage("Tên phải ít hơn 50 ký tự");
        }
    }
}