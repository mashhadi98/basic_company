using Company.Application.Features.StaticPages.Repositories;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class StaticPageRepository : IStaticPageRepository
{
    private readonly AppDbContext _context;

    public StaticPageRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StaticPage?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.StaticPages
            .FirstOrDefaultAsync(sp => sp.Id == id, cancellationToken);
    }

    public async Task<StaticPage?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.StaticPages
            .FirstOrDefaultAsync(sp => sp.Key == key, cancellationToken);
    }

    public async Task<List<StaticPage>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<StaticPage> query = _context.StaticPages
            .OrderBy(sp => sp.CreatedAt);

        if (isPublished.HasValue)
            query = query.Where(sp => sp.IsPublished == isPublished.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(StaticPage page, CancellationToken cancellationToken = default)
    {
        page.CreatedAt = DateTime.UtcNow;
        await _context.StaticPages.AddAsync(page, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(StaticPage page, CancellationToken cancellationToken = default)
    {
        page.UpdatedAt = DateTime.UtcNow;
        _context.StaticPages.Update(page);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _context.StaticPages.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (page != null)
        {
            _context.StaticPages.Remove(page);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.StaticPages.AnyAsync(sp => sp.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _context.StaticPages.AnyAsync(sp => sp.Key == key, cancellationToken);
    }
}