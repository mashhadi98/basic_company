using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly AppDbContext _context;

    public ProductCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(pc => pc.ChildCategories)
            .FirstOrDefaultAsync(pc => pc.Id == id, cancellationToken);
    }

    public async Task<ProductCategory?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .FirstOrDefaultAsync(pc => pc.Slug == slug, cancellationToken);
    }

    public async Task<List<ProductCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Include(pc => pc.ChildCategories)
            .OrderBy(pc => pc.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProductCategory>> GetRootCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Where(pc => pc.ParentCategoryId == null)
            .Include(pc => pc.ChildCategories)
            .OrderBy(pc => pc.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ProductCategory>> GetChildCategoriesAsync(Guid parentId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductCategories
            .Where(pc => pc.ParentCategoryId == parentId)
            .OrderBy(pc => pc.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        category.CreatedAt = DateTime.UtcNow;
        await _context.ProductCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        category.UpdatedAt = DateTime.UtcNow;
        _context.ProductCategories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.ProductCategories.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (category != null)
        {
            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
