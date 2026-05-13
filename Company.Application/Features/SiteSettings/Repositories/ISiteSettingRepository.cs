using Company.Domain.Entities;

namespace Company.Application.Features.SiteSettings.Repositories;

public interface ISiteSettingRepository
{
    Task<SiteSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<SiteSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<List<SiteSetting>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task AddAsync(SiteSetting siteSetting, CancellationToken cancellationToken = default);
    Task UpdateAsync(SiteSetting siteSetting, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
