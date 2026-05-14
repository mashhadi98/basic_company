using Company.Domain.Entities;

namespace Company.Application.Features.Blog.Repositories;

public interface IBlogCategoryRepository
{
    Task<BlogCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BlogCategory?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<BlogCategory>> GetAllAsync(bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task AddAsync(BlogCategory category, CancellationToken cancellationToken = default);
    Task UpdateAsync(BlogCategory category, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IBlogPostRepository
{
    Task<BlogPost?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<BlogPost>> GetAllAsync(Guid? categoryId = null, bool? isPublished = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task AddAsync(BlogPost post, CancellationToken cancellationToken = default);
    Task UpdateAsync(BlogPost post, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface IBlogCommentRepository
{
    Task<BlogComment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<BlogComment>> GetByPostIdAsync(Guid postId, bool? isApproved = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task<List<BlogComment>> GetAllAsync(bool? isApproved = null, int? skip = null, int? take = null, CancellationToken cancellationToken = default);
    Task AddAsync(BlogComment comment, CancellationToken cancellationToken = default);
    Task UpdateAsync(BlogComment comment, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
