using Company.Application.Features.Products.Commands;
using FluentValidation;

namespace Company.Application.Features.Products.Validators;

public class DeleteProductAttributeCommandValidator : AbstractValidator<DeleteProductAttributeCommand>
{
    public DeleteProductAttributeCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("شناسه محصول الزامی است.");

        RuleFor(x => x.AttributeId)
            .NotEmpty()
            .WithMessage("شناسه ویژگی الزامی است.");
    }
}
