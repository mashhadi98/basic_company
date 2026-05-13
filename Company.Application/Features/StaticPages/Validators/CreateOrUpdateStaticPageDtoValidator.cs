using Company.Application.Features.StaticPages.DTOs;
using FluentValidation;

namespace Company.Application.Features.StaticPages.Validators;

public class CreateOrUpdateStaticPageDtoValidator : AbstractValidator<CreateOrUpdateStaticPageDto>
{
    public CreateOrUpdateStaticPageDtoValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("کلید الزامی است.")
            .MaximumLength(100).WithMessage("کلید نمی‌تواند بیش از ۱۰۰ کاراکتر باشد.")
            .Matches("^[a-zA-Z0-9_-]+$").WithMessage("کلید فقط می‌تواند شامل حروف انگلیسی، اعداد، خط تیره و زیرخط باشد.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان الزامی است.")
            .MaximumLength(200).WithMessage("عنوان نمی‌تواند بیش از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Summary)
            .NotEmpty().WithMessage("خلاصه الزامی است.")
            .MaximumLength(500).WithMessage("خلاصه نمی‌تواند بیش از ۵۰۰ کاراکتر باشد.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("توضیحات الزامی است.");

        RuleFor(x => x.Image)
            .MaximumLength(500).WithMessage("مسیر تصویر نمی‌تواند بیش از ۵۰۰ کاراکتر باشد.")
            .When(x => !string.IsNullOrEmpty(x.Image));
    }
}