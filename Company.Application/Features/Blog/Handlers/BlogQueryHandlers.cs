using Company.Application.Features.Blog.DTOs;
using Company.Application.Features.Blog.Queries;
using Company.Application.Features.Blog.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Blog.Handlers;

// Blog Category Query Handlers
public class GetBlogCategoriesQueryHandler : IRequestHandler<GetBlogCategoriesQuery, List<BlogCategoryDto>>
{
    private readonly IBlogCategoryRepository _repository;

    public GetBlogCategoriesQueryHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BlogCategoryDto>> Handle(GetBlogCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(request.IsPublished, request.Skip, request.Take, cancellationToken);
        return categories.Select(MapToDto).ToList();
    }

    private static BlogCategoryDto MapToDto(Domain.Entities.BlogCategory category)
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

public class GetBlogCategoryByIdQueryHandler : IRequestHandler<GetBlogCategoryByIdQuery, BlogCategoryDto?>
{
    private readonly IBlogCategoryRepository _repository;

    public GetBlogCategoryByIdQueryHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogCategoryDto?> Handle(GetBlogCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            return null;

        return MapToDto(category);
    }

    private static BlogCategoryDto MapToDto(Domain.Entities.BlogCategory category)
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

public class GetBlogCategoryBySlugQueryHandler : IRequestHandler<GetBlogCategoryBySlugQuery, BlogCategoryDto?>
{
    private readonly IBlogCategoryRepository _repository;

    public GetBlogCategoryBySlugQueryHandler(IBlogCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogCategoryDto?> Handle(GetBlogCategoryBySlugQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetBySlugAsync(request.Slug, cancellationToken);
        if (category == null)
            return null;

        return MapToDto(category);
    }

    private static BlogCategoryDto MapToDto(Domain.Entities.BlogCategory category)
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

// Blog Post Query Handlers
public class GetBlogPostsQueryHandler : IRequestHandler<GetBlogPostsQuery, List<BlogPostDto>>
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostsQueryHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BlogPostDto>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = await _repository.GetAllAsync(request.CategoryId, request.IsPublished, request.Skip, request.Take, cancellationToken);
        return posts.Select(MapToDto).ToList();
    }

    private static BlogPostDto MapToDto(Domain.Entities.BlogPost post)
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

public class GetBlogPostByIdQueryHandler : IRequestHandler<GetBlogPostByIdQuery, BlogPostDto?>
{
    private readonly IBlogPostRepository _repository;
    private readonly IBlogCommentRepository _commentRepository;

    public GetBlogPostByIdQueryHandler(IBlogPostRepository repository, IBlogCommentRepository commentRepository)
    {
        _repository = repository;
        _commentRepository = commentRepository;
    }

    public async Task<BlogPostDto?> Handle(GetBlogPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
            return null;

        var comments = await _commentRepository.GetByPostIdAsync(request.PostId, isApproved: true, cancellationToken: cancellationToken);
        return MapToDto(post, comments.ToList());
    }

    private static BlogPostDto MapToDto(Domain.Entities.BlogPost post, List<Domain.Entities.BlogComment> comments)
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
            UpdatedAt = post.UpdatedAt,
            Comments = comments.Select(c => new BlogCommentDto
            {
                Id = c.Id,
                AuthorName = c.AuthorName,
                AuthorEmail = c.AuthorEmail,
                Content = c.Content,
                BlogPostId = c.BlogPostId,
                IsApproved = c.IsApproved,
                SortOrder = c.SortOrder,
                CreatedAt = c.CreatedAt
            }).ToList()
        };
    }
}

public class GetBlogPostBySlugQueryHandler : IRequestHandler<GetBlogPostBySlugQuery, BlogPostDto?>
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostBySlugQueryHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPostDto?> Handle(GetBlogPostBySlugQuery request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetBySlugAsync(request.Slug, cancellationToken);
        if (post == null)
            return null;

        return MapToDto(post);
    }

    private static BlogPostDto MapToDto(Domain.Entities.BlogPost post)
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

// Blog Comment Query Handlers
public class GetBlogCommentsQueryHandler : IRequestHandler<GetBlogCommentsQuery, List<BlogCommentDto>>
{
    private readonly IBlogCommentRepository _repository;

    public GetBlogCommentsQueryHandler(IBlogCommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BlogCommentDto>> Handle(GetBlogCommentsQuery request, CancellationToken cancellationToken)
    {
        List<BlogComment> comments;

        if (request.PostId.HasValue)
        {
            comments = await _repository.GetByPostIdAsync(request.PostId.Value, request.IsApproved, request.Skip, request.Take, cancellationToken);
        }
        else
        {
            comments = await _repository.GetAllAsync(request.IsApproved, request.Skip, request.Take, cancellationToken);
        }

        return comments.Select(MapToDto).ToList();
    }

    private static BlogCommentDto MapToDto(Domain.Entities.BlogComment comment)
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

public class GetBlogCommentByIdQueryHandler : IRequestHandler<GetBlogCommentByIdQuery, BlogCommentDto?>
{
    private readonly IBlogCommentRepository _repository;

    public GetBlogCommentByIdQueryHandler(IBlogCommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogCommentDto?> Handle(GetBlogCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await _repository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment == null)
            return null;

        return MapToDto(comment);
    }

    private static BlogCommentDto MapToDto(Domain.Entities.BlogComment comment)
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
