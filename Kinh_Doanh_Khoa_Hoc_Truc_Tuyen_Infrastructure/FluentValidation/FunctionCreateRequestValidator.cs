using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Systems;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class FunctionCreateRequestValidator : AbstractValidator<FunctionCreateRequest>
    {
        public FunctionCreateRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Yêu cầu nhập id").MaximumLength(50)
                .WithMessage("Id không được vượt quá 50 ký tự");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Yêu cầu nhập tên").MaximumLength(200)
                .WithMessage("Tên không được vượt quá 200 ký tự");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Yêu cầu nhập đường dẫn").MaximumLength(200)
                .WithMessage("Đường dẫn không được vượt quá 200 ký tự");
        }
    }
}