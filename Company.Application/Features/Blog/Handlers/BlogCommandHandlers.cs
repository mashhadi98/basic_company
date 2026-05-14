using Company.Application.Features.Blog.Commands;
using Company.Application.Features.Blog.DTOs;
using Company.Application.Features.Blog.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Blog.Handlers;

// Blog Category Handlers
public class CreateBlogCategoryCommandHandler : IRequestHandler<CreateBlogCategoryCommand, BlogCategoryDto>
{
    private readonly IBlogCategoryRepository _repository;

    public CreateBlogCategoryCommandHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogCategoryDto> Handle(CreateBlogCategoryCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CategoryData;
        var category = new BlogCategory
        {
            Title = dto.Title,
            Slug = dto.Slug,
            Description = dto.Description,
            SortOrder = dto.SortOrder,
            IsPublished = dto.IsPublished,
            SeoMetaTitle = dto.SeoMetaTitle,
            SeoMetaDescription = dto.SeoMetaDescription,
            SeoKeywords = dto.SeoKeywords,
            CanonicalUrl = dto.CanonicalUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(category, cancellationToken);
        return MapToDto(category);
    }

    private static BlogCategoryDto MapToDto(BlogCategory category)
    {
        return new BlogCategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            Description = category.Description,
            SortOrder = category.SortOrder,
            IsPublished = category.IsPublished,
            SeoMetaTitle = category.SeoMetaTitle,
            SeoMetaDescription = category.SeoMetaDescription,
            SeoKeywords = category.SeoKeywords,
            CanonicalUrl = category.CanonicalUrl,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}

public class UpdateBlogCategoryCommandHandler : IRequestHandler<UpdateBlogCategoryCommand, BlogCategoryDto>
{
    private readonly IBlogCategoryRepository _repository;

    public UpdateBlogCategoryCommandHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogCategoryDto> Handle(UpdateBlogCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            throw new InvalidOperationException($"دسته‌بندی بلاگ با شناسه {request.CategoryId} یافت نشد.");

        var dto = request.CategoryData;
        category.Title = dto.Title;
        category.Slug = dto.Slug;
        category.Description = dto.Description;
        category.SortOrder = dto.SortOrder;
        category.IsPublished = dto.IsPublished;
        category.SeoMetaTitle = dto.SeoMetaTitle;
        category.SeoMetaDescription = dto.SeoMetaDescription;
        category.SeoKeywords = dto.SeoKeywords;
        category.CanonicalUrl = dto.CanonicalUrl;

        await _repository.UpdateAsync(category, cancellationToken);
        return MapToDto(category);
    }

    private static BlogCategoryDto MapToDto(BlogCategory category)
    {
        return new BlogCategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            Description = category.Description,
            SortOrder = category.SortOrder,
            IsPublished = category.IsPublished,
            SeoMetaTitle = category.SeoMetaTitle,
            SeoMetaDescription = category.SeoMetaDescription,
            SeoKeywords = category.SeoKeywords,
            CanonicalUrl = category.CanonicalUrl,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}

public class DeleteBlogCategoryCommandHandler : IRequestHandler<DeleteBlogCategoryCommand, bool>
{
    private readonly IBlogCategoryRepository _repository;

    public DeleteBlogCategoryCommandHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBlogCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.CategoryId, cancellationToken);
        if (!exists)
            return false;

        await _repository.DeleteAsync(request.CategoryId, cancellationToken);
        return true;
    }
}

public class ToggleBlogCategoryPublishCommandHandler : IRequestHandler<ToggleBlogCategoryPublishCommand, bool>
{
    private readonly IBlogCategoryRepository _repository;

    public ToggleBlogCategoryPublishCommandHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleBlogCategoryPublishCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            return false;

        category.IsPublished = !category.IsPublished;
        await _repository.UpdateAsync(category, cancellationToken);
        return true;
    }
}

// Blog Post Handlers
public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand, BlogPostDto>
{
    private readonly IBlogPostRepository _repository;

    public CreateBlogPostCommandHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var dto = request.PostData;
        var post = new BlogPost
        {
            Title = dto.Title,
            Slug = dto.Slug,
            Content = dto.Content,
            Summary = dto.Summary,
            FeaturedImage = dto.FeaturedImage,
            CategoryId = dto.CategoryId,
            Author = dto.Author,
            PublishedDate = dto.PublishedDate,
            IsPublished = dto.IsPublished,
            AllowComments = dto.AllowComments,
            SortOrder = dto.SortOrder,
            SeoMetaTitle = dto.SeoMetaTitle,
            SeoMetaDescription = dto.SeoMetaDescription,
            SeoKeywords = dto.SeoKeywords,
            CanonicalUrl = dto.CanonicalUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(post, cancellationToken);
        return MapToDto(post);
    }

    private static BlogPostDto MapToDto(BlogPost post)
    {
        return new BlogPostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Content = post.Content,
            Summary = post.Summary,
            FeaturedImage = post.FeaturedImage,
            CategoryId = post.CategoryId,
            Author = post.Author,
            PublishedDate = post.PublishedDate,
            IsPublished = post.IsPublished,
            AllowComments = post.AllowComments,
            ViewCount = post.ViewCount,
            SortOrder = post.SortOrder,
            SeoMetaTitle = post.SeoMetaTitle,
            SeoMetaDescription = post.SeoMetaDescription,
            SeoKeywords = post.SeoKeywords,
            CanonicalUrl = post.CanonicalUrl,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }
}

public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand, BlogPostDto>
{
    private readonly IBlogPostRepository _repository;

    public UpdateBlogPostCommandHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
            throw new InvalidOperationException($"مقاله بلاگ با شناسه {request.PostId} یافت نشد.");

        var dto = request.PostData;
        post.Title = dto.Title;
        post.Slug = dto.Slug;
        post.Content = dto.Content;
        post.Summary = dto.Summary;
        post.FeaturedImage = dto.FeaturedImage;
        post.CategoryId = dto.CategoryId;
        post.Author = dto.Author;
        post.PublishedDate = dto.PublishedDate;
        post.IsPublished = dto.IsPublished;
        post.AllowComments = dto.AllowComments;
        post.SortOrder = dto.SortOrder;
        post.SeoMetaTitle = dto.SeoMetaTitle;
        post.SeoMetaDescription = dto.SeoMetaDescription;
        post.SeoKeywords = dto.SeoKeywords;
        post.CanonicalUrl = dto.CanonicalUrl;

        await _repository.UpdateAsync(post, cancellationToken);
        return MapToDto(post);
    }

    private static BlogPostDto MapToDto(BlogPost post)
    {
        return new BlogPostDto
        {
            Id = post.Id,
            Title = post.Title,
            Slug = post.Slug,
            Content = post.Content,
            Summary = post.Summary,
            FeaturedImage = post.FeaturedImage,
            CategoryId = post.CategoryId,
            Author = post.Author,
            PublishedDate = post.PublishedDate,
            IsPublished = post.IsPublished,
            AllowComments = post.AllowComments,
            ViewCount = post.ViewCount,
            SortOrder = post.SortOrder,
            SeoMetaTitle = post.SeoMetaTitle,
            SeoMetaDescription = post.SeoMetaDescription,
            SeoKeywords = post.SeoKeywords,
            CanonicalUrl = post.CanonicalUrl,
            CreatedAt = post.CreatedAt,
            UpdatedAt = post.UpdatedAt
        };
    }
}

public class DeleteBlogPostCommandHandler : IRequestHandler<DeleteBlogPostCommand, bool>
{
    private readonly IBlogPostRepository _repository;

    public DeleteBlogPostCommandHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.PostId, cancellationToken);
        if (!exists)
            return false;

        await _repository.DeleteAsync(request.PostId, cancellationToken);
        return true;
    }
}

public class ToggleBlogPostPublishCommandHandler : IRequestHandler<ToggleBlogPostPublishCommand, bool>
{
    private readonly IBlogPostRepository _repository;

    public ToggleBlogPostPublishCommandHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleBlogPostPublishCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
            return false;

        post.IsPublished = !post.IsPublished;
        await _repository.UpdateAsync(post, cancellationToken);
        return true;
    }
}

public class IncrementBlogPostViewCommandHandler : IRequestHandler<IncrementBlogPostViewCommand, bool>
{
    private readonly IBlogPostRepository _repository;

    public IncrementBlogPostViewCommandHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(IncrementBlogPostViewCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
            return false;

        post.ViewCount++;
        await _repository.UpdateAsync(post, cancellationToken);
        return true;
    }
}

// Blog Comment Handlers
public class CreateBlogCommentCommandHandler : IRequestHandler<CreateBlogCommentCommand, BlogCommentDto>
{
    private readonly IBlogCommentRepository _repository;

    public CreateBlogCommentCommandHandler(IBlogCommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogCommentDto> Handle(CreateBlogCommentCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CommentData;
        var comment = new BlogComment
        {
            AuthorName = dto.AuthorName,
            AuthorEmail = dto.AuthorEmail,
            Content = dto.Content,
            BlogPostId = dto.BlogPostId,
            IsApproved = false,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(comment, cancellationToken);
        return MapToDto(comment);
    }

    private static BlogCommentDto MapToDto(BlogComment comment)
    {
        return new BlogCommentDto
        {
            Id = comment.Id,
            AuthorName = comment.AuthorName,
            AuthorEmail = comment.AuthorEmail,
            Content = comment.Content,
            BlogPostId = comment.BlogPostId,
            IsApproved = comment.IsApproved,
            SortOrder = comment.SortOrder,
            CreatedAt = comment.CreatedAt
        };
    }
}

public class ApproveBlogCommentCommandHandler : IRequestHandler<ApproveBlogCommentCommand, bool>
{
    private readonly IBlogCommentRepository _repository;

    public ApproveBlogCommentCommandHandler(IBlogCommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ApproveBlogCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment == null)
            return false;

        comment.IsApproved = true;
        await _repository.UpdateAsync(comment, cancellationToken);
        return true;
    }
}

public class RejectBlogCommentCommandHandler : IRequestHandler<RejectBlogCommentCommand, bool>
{
    private readonly IBlogCommentRepository _repository;

    public RejectBlogCommentCommandHandler(IBlogCommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(RejectBlogCommentCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.CommentId, cancellationToken);
        if (!exists)
            return false;

        await _repository.DeleteAsync(request.CommentId, cancellationToken);
        return true;
    }
}
