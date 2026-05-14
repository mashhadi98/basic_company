using Company.Application.Features.Customers.DTOs;
using Company.Domain.Entities;

namespace Company.Application.Features.Customers.Handlers;

internal static class CustomerMapper
{
    public static CustomerDto ToDto(Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            CompanyName = customer.CompanyName,
            Description = customer.Description,
            CompanyImage = customer.CompanyImage,
            SortOrder = customer.SortOrder,
            IsPublished = customer.IsPublished,
            CreatedAt = customer.CreatedAt,
            UpdatedAt = customer.UpdatedAt,
            CreatedBy = customer.CreatedBy,
            UpdatedBy = customer.UpdatedBy
        };
    }
}
