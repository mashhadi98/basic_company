using Company.Application.Features.Products.DTOs;
using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور ایجاد دسته‌بندی محصول
/// </summary>
public class CreateProductCategoryCommand : IRequest<ProductCategoryDto>
{
    public CreateOrUpdateProductCategoryDto CategoryData { get; set; } = new();
}
