using Company.Application.Features.Products.DTOs;
using FluentValidation;

namespace Company.Application.Features.Products.Validators;

public class CreateOrUpdateProductDtoValidator : AbstractValidator<CreateOrUpdateProductDto>
{
    public CreateOrUpdateProductDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان محصول الزامی است.")
            .MaximumLength(512)
            .WithMessage("عنوان محصول نمی‌تواند بیشتر از 512 کاراکتر باشد.");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug الزامی است.")
            .MaximumLength(512)
            .WithMessage("Slug نمی‌تواند بیشتر از 512 کاراکتر باشد.");

        RuleFor(x => x.ShortDescription)
            .NotEmpty()
            .WithMessage("توضیح کوتاه الزامی است.")
            .MaximumLength(1000)
            .WithMessage("توضیح کوتاه نمی‌تواند بیشتر از 1000 کاراکتر باشد.");

        RuleFor(x => x.FullDescription)
            .NotEmpty()
            .WithMessage("توضیح کامل الزامی است.");

        RuleFor(x => x.SeoMetaTitle)
            .MaximumLength(160)
            .WithMessage("عنوان Meta SEO نمی‌تواند بیشتر از 160 کاراکتر باشد.");

        RuleFor(x => x.SeoMetaDescription)
            .MaximumLength(160)
            .WithMessage("توضیح Meta SEO نمی‌تواند بیشتر از 160 کاراکتر باشد.");

        RuleForEach(x => x.Attributes)
            .SetValidator(new CreateOrUpdateProductAttributeDtoValidator());
    }
}

public class CreateOrUpdateProductAttributeDtoValidator : AbstractValidator<CreateOrUpdateProductAttributeDto>
{
    public CreateOrUpdateProductAttributeDtoValidator()
    {
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
