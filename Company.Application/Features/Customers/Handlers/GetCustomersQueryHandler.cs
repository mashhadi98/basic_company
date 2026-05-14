using Company.Application.Features.Customers.DTOs;
using Company.Application.Features.Customers.Queries;
using Company.Application.Features.Customers.Repositories;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly ICustomerRepository _repository;

    public GetCustomersQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _repository.GetAllAsync(request.IsPublished, request.Skip, request.Take, cancellationToken);
        return customers.Select(CustomerMapper.ToDto).ToList();
    }
}
