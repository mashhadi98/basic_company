using Company.Application.Features.Customers.Commands;
using Company.Application.Features.Customers.Repositories;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class ReorderCustomersCommandHandler : IRequestHandler<ReorderCustomersCommand, bool>
{
    private readonly ICustomerRepository _repository;

    public ReorderCustomersCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ReorderCustomersCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var customer = await _repository.GetByIdAsync(item.Id, cancellationToken);
            if (customer != null)
            {
                customer.SortOrder = item.SortOrder;
                await _repository.UpdateAsync(customer, cancellationToken);
            }
        }

        return true;
    }
}
