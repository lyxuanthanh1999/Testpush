using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class FeedBackCreateRequestValidator : AbstractValidator<FeedBackCreateRequest>
    {
        public FeedBackCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Yêu cầu nhập họ và tên")
                .MaximumLength(200).WithMessage("Họ và tên không được vượt quá 200 ký tự");

            RuleFor(x => x.Message).NotEmpty().WithMessage("Yêu cầu nhập tin nhắn");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Yêu cầu nhập email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Định dạng email không chính xác");

            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Yêu cầu nhập số điện thoại");
        }
    }
}