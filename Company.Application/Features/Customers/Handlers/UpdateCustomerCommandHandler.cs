using Company.Application.Features.Customers.Commands;
using Company.Application.Features.Customers.DTOs;
using Company.Application.Features.Customers.Repositories;
using MediatR;

namespace Company.Application.Features.Customers.Handlers;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;

    public UpdateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken);
        if (customer == null)
            throw new InvalidOperationException($"Customer with id {request.CustomerId} was not found.");

        var dto = request.CustomerData;
        customer.CompanyName = dto.CompanyName;
        customer.Description = dto.Description;
        customer.CompanyImage = dto.CompanyImage;
        customer.SortOrder = dto.SortOrder;
        customer.IsPublished = dto.IsPublished;

        await _repository.UpdateAsync(customer, cancellationToken);
        return CustomerMapper.ToDto(customer);
    }
}
