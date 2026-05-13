using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class ProductAttributeRepository : IProductAttributeRepository
{
    private readonly AppDbContext _context;

    public ProductAttributeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProductAttribute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductAttributes
            .FirstOrDefaultAsync(pa => pa.Id == id, cancellationToken);
    }

    public async Task<List<ProductAttribute>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductAttributes
            .Where(pa => pa.ProductId == productId)
            .OrderBy(pa => pa.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ProductAttribute attribute, CancellationToken cancellationToken = default)
    {
        await _context.ProductAttributes.AddAsync(attribute, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(ProductAttribute attribute, CancellationToken cancellationToken = default)
    {
        _context.ProductAttributes.Update(attribute);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var attribute = await _context.ProductAttributes.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (attribute != null)
        {
            _context.ProductAttributes.Remove(attribute);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task DeleteByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        var attributes = await _context.ProductAttributes
            .Where(pa => pa.ProductId == productId)
            .ToListAsync(cancellationToken);

        _context.ProductAttributes.RemoveRange(attributes);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductAttributes
            .AnyAsync(pa => pa.Id == id, cancellationToken);
    }
}
