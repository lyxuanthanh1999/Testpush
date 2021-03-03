using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class LessonViewModelRequestValidator : AbstractValidator<LessonCreateRequest>
    {
        public LessonViewModelRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Yêu cầu nhập tên bài học");
        }
    }
}