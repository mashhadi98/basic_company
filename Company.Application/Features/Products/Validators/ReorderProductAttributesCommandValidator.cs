using Company.Application.Features.Products.Commands;
using FluentValidation;

namespace Company.Application.Features.Products.Validators;

public class ReorderProductAttributesCommandValidator : AbstractValidator<ReorderProductAttributesCommand>
{
    public ReorderProductAttributesCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("شناسه محصول الزامی است.");

        RuleFor(x => x.Attributes)
            .NotEmpty()
            .WithMessage("حداقل یک ویژگی برای تغییر ترتیب الزامی است.");

        RuleForEach(x => x.Attributes)
            .SetValidator(new ReorderAttributeItemValidator());
    }
}

public class ReorderAttributeItemValidator : AbstractValidator<ReorderAttributeItem>
{
    public ReorderAttributeItemValidator()
    {
        RuleFor(x => x.AttributeId)
            .NotEmpty()
            .WithMessage("شناسه ویژگی الزامی است.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage("ترتیب نمی‌تواند منفی باشد.");
    }
}
