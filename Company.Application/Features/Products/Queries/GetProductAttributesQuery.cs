using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Queries;

/// <summary>
/// سؤال دریافت تمام ویژگی‌های محصول
/// </summary>
public class GetProductAttributesQuery : IRequest<List<ProductAttributeDto>>
{
    public Guid ProductId { get; set; }
}
