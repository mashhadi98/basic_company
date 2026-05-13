using Company.Application.Features.CompanyFeatures.Commands;
using FluentValidation;

namespace Company.Application.Features.CompanyFeatures.Validators;

public class CreateCompanyFeatureCommandValidator : AbstractValidator<CreateCompanyFeatureCommand>
{
    public CreateCompanyFeatureCommandValidator()
    {
        RuleFor(x => x.FeatureData)
            .NotNull().WithMessage("داده ویژگی الزامی است.")
            .SetValidator(new CreateOrUpdateCompanyFeatureDtoValidator());
    }
}

public class UpdateCompanyFeatureCommandValidator : AbstractValidator<UpdateCompanyFeatureCommand>
{
    public UpdateCompanyFeatureCommandValidator()
    {
        RuleFor(x => x.FeatureId)
            .NotEmpty().WithMessage("شناسه ویژگی الزامی است.");

        RuleFor(x => x.FeatureData)
            .NotNull().WithMessage("داده ویژگی الزامی است.")
            .SetValidator(new CreateOrUpdateCompanyFeatureDtoValidator());
    }
}

public class DeleteCompanyFeatureCommandValidator : AbstractValidator<DeleteCompanyFeatureCommand>
{
    public DeleteCompanyFeatureCommandValidator()
    {
        RuleFor(x => x.FeatureId)
            .NotEmpty().WithMessage("شناسه ویژگی الزامی است.");
    }
}

public class ToggleCompanyFeaturePublishCommandValidator : AbstractValidator<ToggleCompanyFeaturePublishCommand>
{
    public ToggleCompanyFeaturePublishCommandValidator()
    {
        RuleFor(x => x.FeatureId)
            .NotEmpty().WithMessage("شناسه ویژگی الزامی است.");
    }
}

public class ReorderCompanyFeaturesCommandValidator : AbstractValidator<ReorderCompanyFeaturesCommand>
{
    public ReorderCompanyFeaturesCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("لیست آیتم‌ها الزامی است.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.Id)
                    .NotEmpty().WithMessage("شناسه آیتم الزامی است.");

                item.RuleFor(i => i.SortOrder)
                    .GreaterThanOrEqualTo(0).WithMessage("ترتیب نمایش باید بزرگتر یا مساوی صفر باشد.");
            });
    }
}