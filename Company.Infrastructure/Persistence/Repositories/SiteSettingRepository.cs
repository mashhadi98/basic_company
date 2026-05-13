using Company.Application.Features.SiteSettings.Repositories;
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class SiteSettingRepository : ISiteSettingRepository
{
    private readonly AppDbContext _context;

    public SiteSettingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SiteSetting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings
            .FirstOrDefaultAsync(ss => ss.Id == id, cancellationToken);
    }

    public async Task<SiteSetting?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings
            .FirstOrDefaultAsync(ss => ss.Key == key, cancellationToken);
    }

    public async Task<List<SiteSetting>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<SiteSetting> query = _context.SiteSettings
            .OrderBy(ss => ss.SortOrder);

        if (isPublished.HasValue)
            query = query.Where(ss => ss.IsPublished == isPublished.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(SiteSetting siteSetting, CancellationToken cancellationToken = default)
    {
        siteSetting.CreatedAt = DateTime.UtcNow;
        await _context.SiteSettings.AddAsync(siteSetting, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SiteSetting siteSetting, CancellationToken cancellationToken = default)
    {
        siteSetting.UpdatedAt = DateTime.UtcNow;
        _context.SiteSettings.Update(siteSetting);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var siteSetting = await _context.SiteSettings.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (siteSetting != null)
        {
            _context.SiteSettings.Remove(siteSetting);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SiteSettings.AnyAsync(ss => ss.Id == id, cancellationToken);
    }
}
