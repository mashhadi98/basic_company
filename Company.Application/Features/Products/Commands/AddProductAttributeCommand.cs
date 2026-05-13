using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور اضافه کردن ویژگی به محصول
/// </summary>
public class AddProductAttributeCommand : IRequest<ProductAttributeDto>
{
    public Guid ProductId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
