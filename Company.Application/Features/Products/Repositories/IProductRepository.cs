using Company.Application.Features.Products.DTOs;
using Company.Domain.Entities;

namespace Company.Application.Features.Products.Repositories;

/// <summary>
/// رابط مخزن محصول
/// </summary>
public interface IProductRepository
{
    Task UpdateWithAttributesAsync(Product product, List<ProductAttributeDto> attributeDtos, CancellationToken cancellationToken = default);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task<List<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
