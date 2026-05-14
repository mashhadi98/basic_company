using Company.Application.Features.Blog.DTOs;
using MediatR;

namespace Company.Application.Features.Blog.Queries;

// Blog Category Queries
public class GetBlogCategoriesQuery : IRequest<List<BlogCategoryDto>>
{
    public bool? IsPublished { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetBlogCategoryByIdQuery : IRequest<BlogCategoryDto?>
{
    public Guid CategoryId { get; set; }
}

public class GetBlogCategoryBySlugQuery : IRequest<BlogCategoryDto?>
{
    public string Slug { get; set; } = string.Empty;
}

// Blog Post Queries
public class GetBlogPostsQuery : IRequest<List<BlogPostDto>>
{
    public Guid? CategoryId { get; set; }
    public bool? IsPublished { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetBlogPostByIdQuery : IRequest<BlogPostDto?>
{
    public Guid PostId { get; set; }
}

public class GetBlogPostBySlugQuery : IRequest<BlogPostDto?>
{
    public string Slug { get; set; } = string.Empty;
}

// Blog Comment Queries
public class GetBlogCommentsQuery : IRequest<List<BlogCommentDto>>
{
    public Guid? PostId { get; set; }
    public bool? IsApproved { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetBlogCommentByIdQuery : IRequest<BlogCommentDto?>
{
    public Guid CommentId { get; set; }
}
