using Company.Application.Features.Blog.Repositories;
using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Persistence.Repositories;

public class BlogCategoryRepository : IBlogCategoryRepository
{
    private readonly AppDbContext _context;

    public BlogCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BlogCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogCategories
            .FirstOrDefaultAsync(bc => bc.Id == id, cancellationToken);
    }

    public async Task<BlogCategory?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.BlogCategories
            .FirstOrDefaultAsync(bc => bc.Slug == slug, cancellationToken);
    }

    public async Task<List<BlogCategory>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<BlogCategory> query = _context.BlogCategories
            .OrderBy(bc => bc.SortOrder);

        if (isPublished.HasValue)
            query = query.Where(bc => bc.IsPublished == isPublished.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(BlogCategory category, CancellationToken cancellationToken = default)
    {
        category.CreatedAt = DateTime.UtcNow;
        await _context.BlogCategories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(BlogCategory category, CancellationToken cancellationToken = default)
    {
        category.UpdatedAt = DateTime.UtcNow;
        _context.BlogCategories.Update(category);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.BlogCategories.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (category != null)
        {
            _context.BlogCategories.Remove(category);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogCategories.AnyAsync(bc => bc.Id == id, cancellationToken);
    }
}

public class BlogPostRepository : IBlogPostRepository
{
    private readonly AppDbContext _context;

    public BlogPostRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogPosts
            .Include(bp => bp.Category)
            .FirstOrDefaultAsync(bp => bp.Id == id, cancellationToken);
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.BlogPosts
            .Include(bp => bp.Category)
            .FirstOrDefaultAsync(bp => bp.Slug == slug, cancellationToken);
    }

    public async Task<List<BlogPost>> GetAllAsync(Guid? categoryId = null, bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<BlogPost> query = _context.BlogPosts
            .Include(bp => bp.Category)
            .OrderByDescending(bp => bp.PublishedDate)
            .ThenBy(bp => bp.SortOrder);

        if (categoryId.HasValue)
            query = query.Where(bp => bp.CategoryId == categoryId.Value);

        if (isPublished.HasValue)
            query = query.Where(bp => bp.IsPublished == isPublished.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(BlogPost post, CancellationToken cancellationToken = default)
    {
        post.CreatedAt = DateTime.UtcNow;
        await _context.BlogPosts.AddAsync(post, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(BlogPost post, CancellationToken cancellationToken = default)
    {
        post.UpdatedAt = DateTime.UtcNow;
        _context.BlogPosts.Update(post);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var post = await _context.BlogPosts.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (post != null)
        {
            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogPosts.AnyAsync(bp => bp.Id == id, cancellationToken);
    }
}

public class BlogCommentRepository : IBlogCommentRepository
{
    private readonly AppDbContext _context;

    public BlogCommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BlogComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogComments
            .FirstOrDefaultAsync(bc => bc.Id == id, cancellationToken);
    }

    public async Task<List<BlogComment>> GetByPostIdAsync(Guid postId, bool? isApproved = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<BlogComment> query = _context.BlogComments
            .Where(bc => bc.BlogPostId == postId)
            .OrderBy(bc => bc.SortOrder)
            .ThenByDescending(bc => bc.CreatedAt);

        if (isApproved.HasValue)
            query = query.Where(bc => bc.IsApproved == isApproved.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<List<BlogComment>> GetAllAsync(bool? isApproved = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default)
    {
        IQueryable<BlogComment> query = _context.BlogComments
            .OrderByDescending(bc => bc.CreatedAt);

        if (isApproved.HasValue)
            query = query.Where(bc => bc.IsApproved == isApproved.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(BlogComment comment, CancellationToken cancellationToken = default)
    {
        comment.CreatedAt = DateTime.UtcNow;
        await _context.BlogComments.AddAsync(comment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(BlogComment comment, CancellationToken cancellationToken = default)
    {
        comment.UpdatedAt = DateTime.UtcNow;
        _context.BlogComments.Update(comment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var comment = await _context.BlogComments.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
        if (comment != null)
        {
            _context.BlogComments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.BlogComments.AnyAsync(bc => bc.Id == id, cancellationToken);
    }
}
