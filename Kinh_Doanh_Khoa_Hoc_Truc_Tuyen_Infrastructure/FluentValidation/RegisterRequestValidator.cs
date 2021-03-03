using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;
using System;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Yêu cầu nhập họ và tên")
                .MaximumLength(200).WithMessage("Họ và tên không được vượt quá 200 ký tự");

            RuleFor(x => x.Dob).NotEmpty().WithMessage("Yêu cầu nhập ngay sinh").GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Ngày sinh không được vượt quá 100 năm");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Yêu cầu nhập email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Định dạng email không chính xác");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Yêu cầu nhập số điện thoại");

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Yêu cầu nhập tên đăng nhập");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Yêu cầu mật khẩu")
                .MinimumLength(4).WithMessage("Mật khẩu phải ít nhất 4 ký tự");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Xác nhận mật khẩu và mật khẩu không khớp");
                }
            });
        }
    }
}