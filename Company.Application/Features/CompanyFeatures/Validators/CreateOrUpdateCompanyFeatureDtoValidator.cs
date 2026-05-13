using Company.Application.Features.CompanyFeatures.DTOs;
using FluentValidation;

namespace Company.Application.Features.CompanyFeatures.Validators;

public class CreateOrUpdateCompanyFeatureDtoValidator : AbstractValidator<CreateOrUpdateCompanyFeatureDto>
{
    public CreateOrUpdateCompanyFeatureDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است.")
            .MaximumLength(200).WithMessage("عنوان نمی‌تواند بیش از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("توضیحات الزامی است.")
            .MaximumLength(500).WithMessage("توضیحات نمی‌تواند بیش از ۵۰۰ کاراکتر باشد.");

        RuleFor(x => x.Icon)
            .NotEmpty().WithMessage("آیکون الزامی است.")
            .MaximumLength(100).WithMessage("آیکون نمی‌تواند بیش از ۱۰۰ کاراکتر باشد.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("ترتیب نمایش باید بزرگتر یا مساوی صفر باشد.");
    }
}