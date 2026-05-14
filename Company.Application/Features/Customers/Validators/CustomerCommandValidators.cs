using Company.Application.Features.Customers.Commands;
using FluentValidation;

namespace Company.Application.Features.Customers.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerData)
            .NotNull()
            .SetValidator(new CreateOrUpdateCustomerDtoValidator());
    }
}

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.CustomerData)
            .NotNull()
            .SetValidator(new CreateOrUpdateCustomerDtoValidator());
    }
}

public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}

public class ToggleCustomerPublishCommandValidator : AbstractValidator<ToggleCustomerPublishCommand>
{
    public ToggleCustomerPublishCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}

public class ReorderCustomersCommandValidator : AbstractValidator<ReorderCustomersCommand>
{
    public ReorderCustomersCommandValidator()
    {
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.Id).NotEmpty();
            item.RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
        });
    }
}
