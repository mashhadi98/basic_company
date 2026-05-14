using Company.Application.Features.Customers.DTOs;
using Company.Application.Features.Customers.Queries;
using Company.Application.Features.Customers.Repositories;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly ICustomerRepository _repository;

    public GetCustomerByIdQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken);
        return customer == null ? null : CustomerMapper.ToDto(customer);
    }
}
