using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class ReorderProductAttributesCommandHandler : IRequestHandler<ReorderProductAttributesCommand, bool>
{
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductRepository _productRepository;

    public ReorderProductAttributesCommandHandler(
        IProductAttributeRepository attributeRepository,
        IProductRepository productRepository)
    {
        _attributeRepository = attributeRepository;
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(ReorderProductAttributesCommand request, CancellationToken cancellationToken)
    {
        // بررسی وجود محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"محصول با شناسه {request.ProductId} یافت نشد.");

        // دریافت تمام ویژگی‌های محصول
        var attributes = await _attributeRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        // به‌روزرسانی ترتیب
        foreach (var item in request.Attributes)
        {
            var attribute = attributes.FirstOrDefault(a => a.Id == item.AttributeId);
            if (attribute != null)
            {
                attribute.SortOrder = item.SortOrder;
                await _attributeRepository.UpdateAsync(attribute, cancellationToken);
            }
        }

        return true;
    }
}
