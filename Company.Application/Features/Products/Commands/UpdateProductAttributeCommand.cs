using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور به‌روزرسانی ویژگی محصول
/// </summary>
public class UpdateProductAttributeCommand : IRequest<ProductAttributeDto>
{
    public Guid ProductId { get; set; }
    public Guid AttributeId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
