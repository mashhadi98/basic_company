using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Queries;

/// <summary>
/// سؤال دریافت یک ویژگی محصول
/// </summary>
public class GetProductAttributeQuery : IRequest<ProductAttributeDto?>
{
    public Guid ProductId { get; set; }
    public Guid AttributeId { get; set; }
}
