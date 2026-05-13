using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور ویرایش دسته‌بندی محصول
/// </summary>
public class UpdateProductCategoryCommand : IRequest<ProductCategoryDto>
{
    public Guid CategoryId { get; set; }
    public CreateOrUpdateProductCategoryDto CategoryData { get; set; } = new();
}
