using Company.Domain.Entities;

namespace Company.Application.Features.Products.Repositories;

/// <summary>
/// رابط مخزن ویژگی محصول
/// </summary>
public interface IProductAttributeRepository
{
    Task<ProductAttribute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<ProductAttribute>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(ProductAttribute attribute, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductAttribute attribute, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
