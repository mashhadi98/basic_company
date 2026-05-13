using Company.Application.Features.Products.DTOs;
using Microsoft.AspNetCore.Http;

namespace Company.Presentation.Areas.Admin.Models;

public class ProductViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public string FullDescription { get; set; } = string.Empty;
    public string? MainImage { get; set; }
    public IFormFile? MainImageFile { get; set; }
    public Guid? CategoryId { get; set; }
    public bool IsFeatured { get; set; }
    public string PublishStatus { get; set; } = "Draft";
    public int SortOrder { get; set; }

    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }

    public string? ThumbnailImage { get; set; }
    public IFormFile? ThumbnailImageFile { get; set; }
    public string? CatalogPdfFile { get; set; }
    public string? VideoUrl { get; set; }

    public List<CreateOrUpdateProductAttributeDto> Attributes { get; set; } = new();
    public List<CreateOrUpdateProductGalleryDto> GalleryImages { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}
