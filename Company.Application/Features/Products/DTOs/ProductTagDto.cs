namespace Company.Application.Features.Products.DTOs;

/// <summary>
/// DTO برای برچسب محصول
/// </summary>
public class ProductTagDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
}
