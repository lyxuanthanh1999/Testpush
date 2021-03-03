using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Yêu cầu nhập mật khẩu hiện tại");

            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yêu cầu nhập mật khẩu mới")
                .MinimumLength(4).WithMessage("Mật khẩu mới ít nhất 4 ký tự");

            RuleFor(x => x).Custom((request, context) =>
                {
                    if (request.NewPassword != request.ConfirmNewPassword)
                    {
                        context.AddFailure("Xác nhận mật khẩu và mật khẩu không khớp");
                    }
                });

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.OldPassword == request.NewPassword)
                {
                    context.AddFailure("Mật khẩu cũ và mật khẩu mới phải khác nhau");
                }
            });
        }
    }
}