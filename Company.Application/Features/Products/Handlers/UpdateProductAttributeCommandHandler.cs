using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class UpdateProductAttributeCommandHandler : IRequestHandler<UpdateProductAttributeCommand, ProductAttributeDto>
{
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductRepository _productRepository;

    public UpdateProductAttributeCommandHandler(
        IProductAttributeRepository attributeRepository,
        IProductRepository productRepository)
    {
        _attributeRepository = attributeRepository;
        _productRepository = productRepository;
    }

    public async Task<ProductAttributeDto> Handle(UpdateProductAttributeCommand request, CancellationToken cancellationToken)
    {
        // بررسی وجود محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"محصول با شناسه {request.ProductId} یافت نشد.");

        // دریافت ویژگی
        var attribute = await _attributeRepository.GetByIdAsync(request.AttributeId, cancellationToken);
        if (attribute == null || attribute.ProductId != request.ProductId)
            throw new InvalidOperationException("ویژگی محصول یافت نشد.");

        // به‌روزرسانی
        attribute.Key = request.Key;
        attribute.Value = request.Value;

        await _attributeRepository.UpdateAsync(attribute, cancellationToken);

        return new ProductAttributeDto
        {
            Id = attribute.Id,
            Key = attribute.Key,
            Value = attribute.Value,
            SortOrder = attribute.SortOrder
        };
    }
}
