using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور حذف دسته‌بندی محصول
/// </summary>
public class DeleteProductCategoryCommand : IRequest<bool>
{
    public Guid CategoryId { get; set; }
}
