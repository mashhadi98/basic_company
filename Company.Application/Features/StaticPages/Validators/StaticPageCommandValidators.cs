using Company.Application.Features.StaticPages.Commands;
using FluentValidation;

namespace Company.Application.Features.StaticPages.Validators;

public class CreateStaticPageCommandValidator : AbstractValidator<CreateStaticPageCommand>
{
    public CreateStaticPageCommandValidator()
    {
        RuleFor(x => x.PageData)
            .NotNull().WithMessage("داده صفحه الزامی است.")
            .SetValidator(new CreateOrUpdateStaticPageDtoValidator());
    }
}

public class UpdateStaticPageCommandValidator : AbstractValidator<UpdateStaticPageCommand>
{
    public UpdateStaticPageCommandValidator()
    {
        RuleFor(x => x.PageId)
            .NotEmpty().WithMessage("شناسه صفحه الزامی است.");

        RuleFor(x => x.PageData)
            .NotNull().WithMessage("داده صفحه الزامی است.")
            .SetValidator(new CreateOrUpdateStaticPageDtoValidator());
    }
}

public class DeleteStaticPageCommandValidator : AbstractValidator<DeleteStaticPageCommand>
{
    public DeleteStaticPageCommandValidator()
    {
        RuleFor(x => x.PageId)
            .NotEmpty().WithMessage("شناسه صفحه الزامی است.");
    }
}

public class ToggleStaticPagePublishCommandValidator : AbstractValidator<ToggleStaticPagePublishCommand>
{
    public ToggleStaticPagePublishCommandValidator()
    {
        RuleFor(x => x.PageId)
            .NotEmpty().WithMessage("شناسه صفحه الزامی است.");
    }
}