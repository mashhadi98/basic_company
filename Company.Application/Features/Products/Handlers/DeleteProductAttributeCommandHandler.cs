using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class DeleteProductAttributeCommandHandler : IRequestHandler<DeleteProductAttributeCommand, bool>
{
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductRepository _productRepository;

    public DeleteProductAttributeCommandHandler(
        IProductAttributeRepository attributeRepository,
        IProductRepository productRepository)
    {
        _attributeRepository = attributeRepository;
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductAttributeCommand request, CancellationToken cancellationToken)
    {
        // بررسی وجود محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"محصول با شناسه {request.ProductId} یافت نشد.");

        // دریافت ویژگی
        var attribute = await _attributeRepository.GetByIdAsync(request.AttributeId, cancellationToken);
        if (attribute == null || attribute.ProductId != request.ProductId)
            throw new InvalidOperationException("ویژگی محصول یافت نشد.");

        await _attributeRepository.DeleteAsync(request.AttributeId, cancellationToken);
        return true;
    }
}
