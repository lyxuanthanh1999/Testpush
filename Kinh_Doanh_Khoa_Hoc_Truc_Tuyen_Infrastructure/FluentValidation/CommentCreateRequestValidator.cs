using FluentValidation;
using Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.ViewModels.Products;

namespace Kinh_Doanh_Khoa_Hoc_Truc_Tuyen_Infrastructure.FluentValidation
{
    public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
    {
        public CommentCreateRequestValidator()
        {
            RuleFor(x => x.EntityId).GreaterThan(0)
                .WithMessage("Không tồn tại EntityId");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Yêu cầu nhập nội dung");

            RuleFor(x => x.EntityType).NotEmpty().WithMessage("Yêu cầu nhập entityType");
        }
    }
}