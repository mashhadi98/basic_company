using Company.Application.Features.Products.Queries;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class GetProductAttributesQueryHandler : IRequestHandler<GetProductAttributesQuery, List<ProductAttributeDto>>
{
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductRepository _productRepository;

    public GetProductAttributesQueryHandler(
        IProductAttributeRepository attributeRepository,
        IProductRepository productRepository)
    {
        _attributeRepository = attributeRepository;
        _productRepository = productRepository;
    }

    public async Task<List<ProductAttributeDto>> Handle(GetProductAttributesQuery request, CancellationToken cancellationToken)
    {
        // بررسی وجود محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"محصول با شناسه {request.ProductId} یافت نشد.");

        // دریافت ویژگی‌ها
        var attributes = await _attributeRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        return attributes
            .OrderBy(a => a.SortOrder)
            .Select(a => new ProductAttributeDto
            {
                Id = a.Id,
                Key = a.Key,
                Value = a.Value,
                SortOrder = a.SortOrder
            })
            .ToList();
    }
}
