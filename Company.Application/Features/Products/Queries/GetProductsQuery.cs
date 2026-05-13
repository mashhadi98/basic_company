using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Queries;

/// <summary>
/// سؤال دریافت تمام محصولات
/// </summary>
public class GetProductsQuery : IRequest<List<ProductDto>>
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public Guid? CategoryId { get; set; }
    public bool? IsPublished { get; set; }
    public string? SearchTerm { get; set; }
}
