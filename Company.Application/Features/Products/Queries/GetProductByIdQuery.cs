using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Queries;

/// <summary>
/// سؤال دریافت یک محصول
/// </summary>
public class GetProductByIdQuery : IRequest<ProductDto?>
{
    public Guid ProductId { get; set; }
}
