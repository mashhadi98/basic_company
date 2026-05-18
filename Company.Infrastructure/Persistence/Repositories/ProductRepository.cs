using Company.Application.Features.Products.DTOs;
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
    public async Task UpdateWithAttributesAsync(Product product,
        List<ProductAttributeDto> attributeDtos,
        CancellationToken cancellationToken = default)
    {
        product.UpdatedAt = DateTime.UtcNow;

        if (attributeDtos != null && attributeDtos.Any())
        {
            var incomingIds = attributeDtos
                .Where(a => a.Id.HasValue)
                .Select(a => a.Id!.Value)
                .ToHashSet();

            // حذف атрибутهای اضافی
            var attributesToRemove = product.Attributes
                .Where(a => !incomingIds.Contains(a.Id))
                .ToList();

            foreach (var attr in attributesToRemove)
            {
                _context.ProductAttributes.Remove(attr);
            }

            // به‌روزرسانی و افزودن جدید (مهم: مستقیم روی DbSet)
            foreach (var attrDto in attributeDtos)
            {
                if (attrDto.Id.HasValue)
                {
                    var existing = product.Attributes.FirstOrDefault(a => a.Id == attrDto.Id.Value);
                    if (existing != null)
                    {
                        existing.Key = attrDto.Key;
                        existing.Value = attrDto.Value;
                        existing.SortOrder = attrDto.SortOrder;
                        continue;
                    }
                }

                // افزودن مستقیم به DbSet (بهترین روش در این سناریو)
                var newAttribute = new ProductAttribute
                {
                    ProductId = product.Id,
                    Key = attrDto.Key,
                    Value = attrDto.Value,
                    SortOrder = attrDto.SortOrder
                };

                _context.ProductAttributes.Add(newAttribute);   // ← تغییر کلیدی اینجا
            }
        }

        // فقط یک SaveChanges
        await _context.SaveChangesAsync(cancellationToken);
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

    public async Task<List<Product>> GetAllAsync(Guid? categoryId = null, bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products
            .Include(p => p.Attributes)
            .Include(p => p.GalleryImages)
            .Include(p => p.Tags)
            .OrderBy(p => p.SortOrder);

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (isPublished.HasValue)
            query = query.Where(p => p.PublishStatus == PublishStatus.Published);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Guid? categoryId = null, bool? isPublished = null, CancellationToken cancellationToken = default)
    {
        IQueryable<Product> query = _context.Products;

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        if (isPublished.HasValue)
            query = query.Where(p => p.PublishStatus == PublishStatus.Published);

        return await query.CountAsync(cancellationToken);
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

        // هیچ Update() اضافی نزنیم — موجودیت قبلاً Track شده
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
