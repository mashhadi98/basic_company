namespace Company.Application.Features.Products.DTOs;

/// <summary>
/// DTO برای محصول
/// </summary>
public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public string? MainImage { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsFeatured { get; set; }
    public string PublishStatus { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }

    public string? ThumbnailImage { get; set; }
    public string? CatalogPdfFile { get; set; }
    public string? VideoUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    public List<ProductAttributeDto> Attributes { get; set; } = new();
    public List<ProductGalleryDto> GalleryImages { get; set; } = new();
    public List<ProductTagDto> Tags { get; set; } = new();
}

/// <summary>
/// DTO برای ایجاد یا ویرایش محصول
/// </summary>
public class CreateOrUpdateProductDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public string? MainImage { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsFeatured { get; set; }
    public string PublishStatus { get; set; } = "Draft";
    public int SortOrder { get; set; }

    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }

    public string? ThumbnailImage { get; set; }
    public string? CatalogPdfFile { get; set; }
    public string? VideoUrl { get; set; }

    public List<CreateOrUpdateProductAttributeDto> Attributes { get; set; } = new();
    public List<CreateOrUpdateProductGalleryDto> GalleryImages { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}
