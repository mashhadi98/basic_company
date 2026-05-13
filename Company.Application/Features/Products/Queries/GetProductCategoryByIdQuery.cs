using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Queries;

/// <summary>
/// سوال برای دریافت یک دسته‌بندی بر اساس شناسه
/// </summary>
public class GetProductCategoryByIdQuery : IRequest<ProductCategoryDto?>
{
    public Guid CategoryId { get; set; }
}
