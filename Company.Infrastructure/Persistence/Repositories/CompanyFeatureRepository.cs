using Company.Application.Features.CompanyFeatures.Repositories;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class CompanyFeatureRepository : ICompanyFeatureRepository
{
    private readonly AppDbContext _context;

    public CompanyFeatureRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CompanyFeature?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyFeatures
            .FirstOrDefaultAsync(cf => cf.Id == id, cancellationToken);
    }

    public async Task<List<CompanyFeature>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<CompanyFeature> query = _context.CompanyFeatures
            .OrderBy(cf => cf.SortOrder);

        if (isPublished.HasValue)
            query = query.Where(cf => cf.IsPublished == isPublished.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(CompanyFeature feature, CancellationToken cancellationToken = default)
    {
        feature.CreatedAt = DateTime.UtcNow;
        await _context.CompanyFeatures.AddAsync(feature, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(CompanyFeature feature, CancellationToken cancellationToken = default)
    {
        feature.UpdatedAt = DateTime.UtcNow;
        _context.CompanyFeatures.Update(feature);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feature = await _context.CompanyFeatures.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (feature != null)
        {
            _context.CompanyFeatures.Remove(feature);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CompanyFeatures.AnyAsync(cf => cf.Id == id, cancellationToken);
    }
}