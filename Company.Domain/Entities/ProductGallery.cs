using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// گالری تصاویر محصول
/// </summary>
public class ProductGallery : BaseEntity
{
    public Guid ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }

    // Navigation Properties
    public Product? Product { get; set; }
}
