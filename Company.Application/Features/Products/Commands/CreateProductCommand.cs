using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور ایجاد محصول با ویژگی‌های آن
/// </summary>
public class CreateProductCommand : IRequest<ProductDto>
{
    public CreateOrUpdateProductDto ProductData { get; set; } = new();
}
