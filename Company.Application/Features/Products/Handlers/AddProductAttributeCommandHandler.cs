using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class AddProductAttributeCommandHandler : IRequestHandler<AddProductAttributeCommand, ProductAttributeDto>
{
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductRepository _productRepository;

    public AddProductAttributeCommandHandler(
        IProductAttributeRepository attributeRepository,
        IProductRepository productRepository)
    {
        _attributeRepository = attributeRepository;
        _productRepository = productRepository;
    }

    public async Task<ProductAttributeDto> Handle(AddProductAttributeCommand request, CancellationToken cancellationToken)
    {
        // بررسی وجود محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"محصول با شناسه {request.ProductId} یافت نشد.");

        // دریافت آخرین ترتیب
        var existingAttributes = await _attributeRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
        var maxSortOrder = existingAttributes.Any() ? existingAttributes.Max(a => a.SortOrder) : -1;

        // ایجاد ویژگی جدید
        var attribute = new ProductAttribute
        {
            ProductId = request.ProductId,
            Key = request.Key,
            Value = request.Value,
            SortOrder = maxSortOrder + 1
        };

        await _attributeRepository.AddAsync(attribute, cancellationToken);

        return new ProductAttributeDto
        {
            Id = attribute.Id,
            Key = attribute.Key,
            Value = attribute.Value,
            SortOrder = attribute.SortOrder
        };
    }
}
