using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductAttributeRepository _attributeRepository;
    private readonly IProductCategoryRepository _categoryRepository;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IProductAttributeRepository attributeRepository,
        IProductCategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _attributeRepository = attributeRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var dto = request.ProductData;

        // بررسی دسته‌بندی
        if (dto.CategoryId.HasValue)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, cancellationToken);
            if (category == null)
                throw new InvalidOperationException("دسته‌بندی محصول یافت نشد.");
        }

        // ایجاد محصول
        var product = new Product
        {
            Title = dto.Title,
            Slug = dto.Slug,
            ShortDescription = dto.ShortDescription,
            FullDescription = dto.FullDescription,
            MainImage = dto.MainImage,
            CategoryId = dto.CategoryId,
            IsFeatured = dto.IsFeatured,
            PublishStatus = Enum.Parse<PublishStatus>(dto.PublishStatus),
            SortOrder = dto.SortOrder,
            SeoMetaTitle = dto.SeoMetaTitle,
            SeoMetaDescription = dto.SeoMetaDescription,
            SeoKeywords = dto.SeoKeywords,
            CanonicalUrl = dto.CanonicalUrl,
            ThumbnailImage = dto.ThumbnailImage,
            CatalogPdfFile = dto.CatalogPdfFile,
            VideoUrl = dto.VideoUrl
        };

        await _productRepository.AddAsync(product, cancellationToken);

        // اضافه کردن ویژگی‌ها
        if (dto.Attributes != null && dto.Attributes.Count > 0)
        {
            foreach (var attr in dto.Attributes)
            {
                var productAttr = new ProductAttribute
                {
                    ProductId = product.Id,
                    Key = attr.Key,
                    Value = attr.Value,
                    SortOrder = attr.SortOrder
                };
                await _attributeRepository.AddAsync(productAttr, cancellationToken);
            }
        }

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
