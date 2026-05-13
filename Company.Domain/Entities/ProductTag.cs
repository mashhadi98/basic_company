using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// برچسب‌های محصول
/// </summary>
public class ProductTag : BaseEntity
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    // Navigation Properties
    public Product? Product { get; set; }
}
