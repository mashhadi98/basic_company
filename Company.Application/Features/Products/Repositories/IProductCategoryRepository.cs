using Company.Domain.Entities;

namespace Company.Application.Features.Products.Repositories;

/// <summary>
/// رابط مخزن دسته‌بندی محصول
/// </summary>
public interface IProductCategoryRepository
{
    Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProductCategory?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<ProductCategory>> GetRootCategoriesAsync(CancellationToken cancellationToken = default);
    Task<List<ProductCategory>> GetChildCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default);
    Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductCategory category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
