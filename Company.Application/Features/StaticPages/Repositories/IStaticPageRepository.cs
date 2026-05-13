using Company.Domain.Entities;

namespace Company.Application.Features.StaticPages.Repositories;

/// <summary>
/// رابط مخزن صفحه ثابت
/// </summary>
public interface IStaticPageRepository
{
    Task<StaticPage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<StaticPage?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<List<StaticPage>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task AddAsync(StaticPage page, CancellationToken cancellationToken = default);
    Task UpdateAsync(StaticPage page, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken = default);
}