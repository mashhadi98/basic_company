using Company.Application.Features.Products.Commands;
using FluentValidation;

namespace Company.Application.Features.Products.Validators;

public class AddProductAttributeCommandValidator : AbstractValidator<AddProductAttributeCommand>
{
    public AddProductAttributeCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("شناسه محصول الزامی است.");

        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("کلید ویژگی الزامی است.")
            .MaximumLength(256)
            .WithMessage("کلید ویژگی نمی‌تواند بیشتر از 256 کاراکتر باشد.");

        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("مقدار ویژگی الزامی است.")
            .MaximumLength(1000)
            .WithMessage("مقدار ویژگی نمی‌تواند بیشتر از 1000 کاراکتر باشد.");
    }
}
