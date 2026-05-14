using Company.Application.Features.Customers.DTOs;
using MediatR;

namespace Company.Application.Features.Customers.Queries;

public class GetCustomersQuery : IRequest<List<CustomerDto>>
{
    public bool? IsPublished { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetCustomerByIdQuery : IRequest<CustomerDto?>
{
    public Guid CustomerId { get; set; }
}
