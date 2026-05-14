using Company.Application.Features.Customers.DTOs;
using MediatR;

namespace Company.Application.Features.Customers.Commands;

public class CreateCustomerCommand : IRequest<CustomerDto>
{
    public CreateOrUpdateCustomerDto CustomerData { get; set; } = new();
}

public class UpdateCustomerCommand : IRequest<CustomerDto>
{
    public Guid CustomerId { get; set; }
    public CreateOrUpdateCustomerDto CustomerData { get; set; } = new();
}

public class DeleteCustomerCommand : IRequest<bool>
{
    public Guid CustomerId { get; set; }
}

public class ToggleCustomerPublishCommand : IRequest<bool>
{
    public Guid CustomerId { get; set; }
}

public class ReorderCustomersCommand : IRequest<bool>
{
    public List<ReorderItem> Items { get; set; } = new();

    public class ReorderItem
    {
        public Guid Id { get; set; }
        public int SortOrder { get; set; }
    }
}
