using MediatR;

namespace Company.Application.Features.Products.Commands;

/// <summary>
/// دستور تغییر ترتیب ویژگی‌های محصول
/// </summary>
public class ReorderProductAttributesCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }
    public List<ReorderAttributeItem> Attributes { get; set; } = new();
}

public class ReorderAttributeItem
{
    public Guid AttributeId { get; set; }
    public int SortOrder { get; set; }
}
