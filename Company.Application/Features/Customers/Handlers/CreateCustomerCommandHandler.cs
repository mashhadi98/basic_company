using Company.Application.Features.Customers.Commands;
using Company.Application.Features.Customers.DTOs;
using Company.Application.Features.Customers.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CustomerData;
        var customer = new Customer
        {
            CompanyName = dto.CompanyName,
            Description = dto.Description,
            CompanyImage = dto.CompanyImage,
            SortOrder = dto.SortOrder,
            IsPublished = dto.IsPublished
        };

        await _repository.AddAsync(customer, cancellationToken);
        return CustomerMapper.ToDto(customer);
    }
}
