using Company.Application.Features.Customers.Commands;
using Company.Application.Features.Customers.Repositories;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class ToggleCustomerPublishCommandHandler : IRequestHandler<ToggleCustomerPublishCommand, bool>
{
    private readonly ICustomerRepository _repository;

    public ToggleCustomerPublishCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleCustomerPublishCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            return false;

        customer.IsPublished = !customer.IsPublished;
        await _repository.UpdateAsync(customer, cancellationToken);
        return true;
    }
}
