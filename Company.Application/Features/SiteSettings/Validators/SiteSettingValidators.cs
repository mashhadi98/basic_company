using Company.Application.Features.SiteSettings.DTOs;
using FluentValidation;

namespace Company.Application.Features.SiteSettings.Validators;

public class CreateOrUpdateSiteSettingDtoValidator : AbstractValidator<CreateOrUpdateSiteSettingDto>
{
    public CreateOrUpdateSiteSettingDtoValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .WithMessage("کلید الزامی است.")
            .MaximumLength(100)
            .WithMessage("کلید نمی‌تواند بیش از 100 کاراکتر باشد.");

        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("مقدار الزامی است.")
            .MaximumLength(5000)
            .WithMessage("مقدار نمی‌تواند بیش از 5000 کاراکتر باشد.");
    }
}

public class CreateSiteSettingCommandValidator : AbstractValidator<Company.Application.Features.SiteSettings.Commands.CreateSiteSettingCommand>
{
    public CreateSiteSettingCommandValidator()
    {
        RuleFor(x => x.SiteSettingData)
            .NotNull()
            .WithMessage("داده‌های تنظیم الزامی است.")
            .SetValidator(new CreateOrUpdateSiteSettingDtoValidator());
    }
}

public class UpdateSiteSettingCommandValidator : AbstractValidator<Company.Application.Features.SiteSettings.Commands.UpdateSiteSettingCommand>
{
    public UpdateSiteSettingCommandValidator()
    {
        RuleFor(x => x.SiteSettingId)
            .NotEmpty()
            .WithMessage("شناسه تنظیم الزامی است.");

        RuleFor(x => x.SiteSettingData)
            .NotNull()
            .WithMessage("داده‌های تنظیم الزامی است.")
            .SetValidator(new CreateOrUpdateSiteSettingDtoValidator());
    }
}

public class DeleteSiteSettingCommandValidator : AbstractValidator<Company.Application.Features.SiteSettings.Commands.DeleteSiteSettingCommand>
{
    public DeleteSiteSettingCommandValidator()
    {
        RuleFor(x => x.SiteSettingId)
            .NotEmpty()
            .WithMessage("شناسه تنظیم الزامی است.");
    }
}

public class ToggleSiteSettingPublishCommandValidator : AbstractValidator<Company.Application.Features.SiteSettings.Commands.ToggleSiteSettingPublishCommand>
{
    public ToggleSiteSettingPublishCommandValidator()
    {
        RuleFor(x => x.SiteSettingId)
            .NotEmpty()
            .WithMessage("شناسه تنظیم الزامی است.");
    }
}
