using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// محصول - محصول کاتالوگ صنعتی برای کارخانهٔ پلاستیک
/// </summary>
public class Product : BaseEntity
{
    // General Information
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public string? MainImage { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsFeatured { get; set; }
    public PublishStatus PublishStatus { get; set; } = PublishStatus.Draft;
    public int SortOrder { get; set; }

    // SEO Fields
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }

    // Media & Files
    public string? ThumbnailImage { get; set; }
    public string? CatalogPdfFile { get; set; }
    public string? VideoUrl { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation Properties
    public ProductCategory? Category { get; set; }
    public ICollection<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    public ICollection<ProductGallery> GalleryImages { get; set; } = new List<ProductGallery>();
    public ICollection<ProductTag> Tags { get; set; } = new List<ProductTag>();
}

/// <summary>
/// وضعیت انتشار محصول
/// </summary>
public enum PublishStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2
}
