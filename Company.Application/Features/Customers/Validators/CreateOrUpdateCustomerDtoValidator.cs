using Company.Application.Features.Customers.DTOs;
using FluentValidation;

namespace Company.Application.Features.Customers.Validators;

public class CreateOrUpdateCustomerDtoValidator : AbstractValidator<CreateOrUpdateCustomerDto>
{
    public CreateOrUpdateCustomerDtoValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty().WithMessage("نام شرکت الزامی است.")
            .MaximumLength(200).WithMessage("نام شرکت نمی‌تواند بیش از ۲۰۰ کاراکتر باشد.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("توضیح همکاری الزامی است.")
            .MaximumLength(500).WithMessage("توضیح همکاری نمی‌تواند بیش از ۵۰۰ کاراکتر باشد.");

        RuleFor(x => x.CompanyImage)
            .MaximumLength(512).WithMessage("مسیر تصویر نمی‌تواند بیش از ۵۱۲ کاراکتر باشد.");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("ترتیب نمایش باید بزرگتر یا مساوی صفر باشد.");
    }
}
