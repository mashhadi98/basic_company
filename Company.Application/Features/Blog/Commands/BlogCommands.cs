using Company.Application.Features.Blog.DTOs;
using MediatR;

namespace Company.Application.Features.Blog.Commands;

// Blog Category Commands
public class CreateBlogCategoryCommand : IRequest<BlogCategoryDto>
{
    public CreateOrUpdateBlogCategoryDto CategoryData { get; set; } = new();
}

public class UpdateBlogCategoryCommand : IRequest<BlogCategoryDto>
{
    public Guid CategoryId { get; set; }
    public CreateOrUpdateBlogCategoryDto CategoryData { get; set; } = new();
}

public class DeleteBlogCategoryCommand : IRequest<bool>
{
    public Guid CategoryId { get; set; }
}

public class ToggleBlogCategoryPublishCommand : IRequest<bool>
{
    public Guid CategoryId { get; set; }
}

// Blog Post Commands
public class CreateBlogPostCommand : IRequest<BlogPostDto>
{
    public CreateOrUpdateBlogPostDto PostData { get; set; } = new();
}

public class UpdateBlogPostCommand : IRequest<BlogPostDto>
{
    public Guid PostId { get; set; }
    public CreateOrUpdateBlogPostDto PostData { get; set; } = new();
}

public class DeleteBlogPostCommand : IRequest<bool>
{
    public Guid PostId { get; set; }
}

public class ToggleBlogPostPublishCommand : IRequest<bool>
{
    public Guid PostId { get; set; }
}

public class IncrementBlogPostViewCommand : IRequest<bool>
{
    public Guid PostId { get; set; }
}

// Blog Comment Commands
public class CreateBlogCommentCommand : IRequest<BlogCommentDto>
{
    public CreateBlogCommentDto CommentData { get; set; } = new();
}

public class ApproveBlogCommentCommand : IRequest<bool>
{
    public Guid CommentId { get; set; }
}

public class RejectBlogCommentCommand : IRequest<bool>
{
    public Guid CommentId { get; set; }
}
