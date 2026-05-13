using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Attributes)
            .Include(p => p.GalleryImages)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Product?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<List<Product>> GetAllAsync(int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .Include(p => p.Attributes)
            .Include(p => p.GalleryImages)
            .Include(p => p.Tags)
            .OrderBy(p => p.SortOrder);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<Product>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Where(p => p.CategoryId == categoryId)
            .Include(p => p.Attributes)
            .Include(p => p.GalleryImages)
            .Include(p => p.Tags)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.CreatedAt = DateTime.UtcNow;
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _context.Products.Update(product);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Products.AnyAsync(p => p.Id == id, cancellationToken);
    }
}
