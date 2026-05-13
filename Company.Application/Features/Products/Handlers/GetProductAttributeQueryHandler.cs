using Company.Application.Features.Products.Queries;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class GetProductAttributeQueryHandler : IRequestHandler<GetProductAttributeQuery, ProductAttributeDto?>
{
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductRepository _productRepository;

    public GetProductAttributeQueryHandler(
        IProductAttributeRepository attributeRepository,
        IProductRepository productRepository)
    {
        _attributeRepository = attributeRepository;
        _productRepository = productRepository;
    }

    public async Task<ProductAttributeDto?> Handle(GetProductAttributeQuery request, CancellationToken cancellationToken)
    {
        // بررسی وجود محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            return null;

        // دریافت ویژگی
        var attribute = await _attributeRepository.GetByIdAsync(request.AttributeId, cancellationToken);
        if (attribute == null || attribute.ProductId != request.ProductId)
            return null;

        return new ProductAttributeDto
        {
            Id = attribute.Id,
            Key = attribute.Key,
            Value = attribute.Value,
            SortOrder = attribute.SortOrder
        };
    }
}
