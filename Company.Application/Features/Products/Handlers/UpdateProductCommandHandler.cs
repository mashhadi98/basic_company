using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public UpdateProductCommandHandler(
        IProductRepository productRepository,
        IProductAttributeRepository attributeRepository,
        IProductCategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _attributeRepository = attributeRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ProductData;

        // دریافت محصول
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);
        if (product == null)
            throw new InvalidOperationException($"محصول با شناسه {request.ProductId} یافت نشد.");

        // بررسی دسته‌بندی
        if (dto.CategoryId.HasValue && dto.CategoryId.Value != product.CategoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, cancellationToken);
            if (category == null)
                throw new InvalidOperationException("دسته‌بندی محصول یافت نشد.");
        }

        // به‌روزرسانی محصول
        product.Title = dto.Title;
        product.Slug = dto.Slug;
        product.ShortDescription = dto.ShortDescription;
        product.FullDescription = dto.FullDescription;
        product.MainImage = dto.MainImage;
        product.CategoryId = dto.CategoryId;
        product.IsFeatured = dto.IsFeatured;
        product.PublishStatus = Enum.Parse<PublishStatus>(dto.PublishStatus);
        product.SortOrder = dto.SortOrder;
        product.SeoMetaTitle = dto.SeoMetaTitle;
        product.SeoMetaDescription = dto.SeoMetaDescription;
        product.SeoKeywords = dto.SeoKeywords;
        product.CanonicalUrl = dto.CanonicalUrl;
        product.ThumbnailImage = dto.ThumbnailImage;
        product.CatalogPdfFile = dto.CatalogPdfFile;
        product.VideoUrl = dto.VideoUrl;

        await _productRepository.UpdateAsync(product, cancellationToken);

        return MapToDto(product);
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Title = product.Title,
            Slug = product.Slug,
            ShortDescription = product.ShortDescription,
            FullDescription = product.FullDescription,
            MainImage = product.MainImage,
            CategoryId = product.CategoryId,
            IsFeatured = product.IsFeatured,
            PublishStatus = product.PublishStatus.ToString(),
            SortOrder = product.SortOrder,
            SeoMetaTitle = product.SeoMetaTitle,
            SeoMetaDescription = product.SeoMetaDescription,
            SeoKeywords = product.SeoKeywords,
            CanonicalUrl = product.CanonicalUrl,
            ThumbnailImage = product.ThumbnailImage,
            CatalogPdfFile = product.CatalogPdfFile,
            VideoUrl = product.VideoUrl,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            CreatedBy = product.CreatedBy,
            UpdatedBy = product.UpdatedBy
        };
    }
}
