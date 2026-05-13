using Company.Domain.Entities;

namespace Company.Application.Features.CompanyFeatures.Repositories;

/// <summary>
/// رابط مخزن ویژگی شرکت
/// </summary>
public interface ICompanyFeatureRepository
{
    Task<CompanyFeature?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<CompanyFeature>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task AddAsync(CompanyFeature feature, CancellationToken cancellationToken = default);
    Task UpdateAsync(CompanyFeature feature, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}