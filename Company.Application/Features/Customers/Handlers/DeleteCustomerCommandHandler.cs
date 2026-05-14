using Company.Application.Features.Customers.Commands;
using Company.Application.Features.Customers.Repositories;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
{
    private readonly ICustomerRepository _repository;

    public DeleteCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            return false;

        await _repository.DeleteAsync(request.CustomerId, cancellationToken);
        return true;
    }
}
