using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور حذف ویژگی محصول
/// </summary>
public class DeleteProductAttributeCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }
    public Guid AttributeId { get; set; }
}
