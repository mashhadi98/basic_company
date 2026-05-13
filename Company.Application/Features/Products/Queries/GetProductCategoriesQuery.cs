using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Queries;

/// <summary>
/// سؤال دریافت تمام دسته‌بندی‌های محصول
/// </summary>
public class GetProductCategoriesQuery : IRequest<List<ProductCategoryDto>>
{
    public bool? IsPublished { get; set; }
    public Guid? ParentCategoryId { get; set; }
}
