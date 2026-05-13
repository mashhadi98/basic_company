using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور به‌روزرسانی محصول
/// </summary>
public class UpdateProductCommand : IRequest<ProductDto>
{
    public Guid ProductId { get; set; }
    public CreateOrUpdateProductDto ProductData { get; set; } = new();
}
