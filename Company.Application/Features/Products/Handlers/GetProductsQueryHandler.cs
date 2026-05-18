using Company.Application.Features.Products.Queries;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
            var products = await _productRepository.GetAllAsync(request.CategoryId, request.IsPublished, request.Skip, request.Take, cancellationToken);
            products = products.Where(p =>
                p.Title.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.ShortDescription.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();

        return products.Select(MapToDto).ToList();
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
            UpdatedBy = product.UpdatedBy,
            Attributes = product.Attributes
                .OrderBy(a => a.SortOrder)
                .Select(a => new ProductAttributeDto
                {
                    Id = a.Id,
                    Key = a.Key,
                    Value = a.Value,
                    SortOrder = a.SortOrder
                })
                .ToList(),
            GalleryImages = product.GalleryImages
                .OrderBy(g => g.SortOrder)
                .Select(g => new ProductGalleryDto
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    AltText = g.AltText,
                    SortOrder = g.SortOrder,
                    IsPrimary = g.IsPrimary
                })
                .ToList(),
            Tags = product.Tags
                .Select(t => new ProductTagDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Slug = t.Slug
                })
                .ToList()
        };
    }
}
