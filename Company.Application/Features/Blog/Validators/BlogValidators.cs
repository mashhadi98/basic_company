using Company.Application.Features.Blog.DTOs;
using FluentValidation;

namespace Company.Application.Features.Blog.Validators;

public class CreateOrUpdateBlogCategoryDtoValidator : AbstractValidator<CreateOrUpdateBlogCategoryDto>
{
    public CreateOrUpdateBlogCategoryDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان دسته‌بندی الزامی است.")
            .MaximumLength(200).WithMessage("عنوان نمی‌تواند بیش از 200 کاراکتر باشد.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug الزامی است.")
            .MaximumLength(200).WithMessage("Slug نمی‌تواند بیش از 200 کاراکتر باشد.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("توضیح الزامی است.")
            .MaximumLength(1000).WithMessage("توضیح نمی‌تواند بیش از 1000 کاراکتر باشد.");

        RuleFor(x => x.SeoMetaTitle)
            .MaximumLength(60).WithMessage("SEO عنوان نمی‌تواند بیش از 60 کاراکتر باشد.");

        RuleFor(x => x.SeoMetaDescription)
            .MaximumLength(160).WithMessage("SEO توضیح نمی‌تواند بیش از 160 کاراکتر باشد.");
    }
}

public class CreateOrUpdateBlogPostDtoValidator : AbstractValidator<CreateOrUpdateBlogPostDto>
{
    public CreateOrUpdateBlogPostDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("عنوان مقاله الزامی است.")
            .MaximumLength(300).WithMessage("عنوان نمی‌تواند بیش از 300 کاراکتر باشد.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug الزامی است.")
            .MaximumLength(300).WithMessage("Slug نمی‌تواند بیش از 300 کاراکتر باشد.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("محتوا الزامی است.");

        RuleFor(x => x.Summary)
            .NotEmpty().WithMessage("خلاصه الزامی است.")
            .MaximumLength(500).WithMessage("خلاصه نمی‌تواند بیش از 500 کاراکتر باشد.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("دسته‌بندی الزامی است.");

        RuleFor(x => x.PublishedDate)
            .NotEmpty().WithMessage("تاریخ انتشار الزامی است.");

        RuleFor(x => x.SeoMetaTitle)
            .MaximumLength(60).WithMessage("SEO عنوان نمی‌تواند بیش از 60 کاراکتر باشد.");

        RuleFor(x => x.SeoMetaDescription)
            .MaximumLength(160).WithMessage("SEO توضیح نمی‌تواند بیش از 160 کاراکتر باشد.");
    }
}

public class CreateBlogCommentDtoValidator : AbstractValidator<CreateBlogCommentDto>
{
    public CreateBlogCommentDtoValidator()
    {
        RuleFor(x => x.AuthorName)
            .NotEmpty().WithMessage("نام نویسنده الزامی است.")
            .MaximumLength(100).WithMessage("نام نویسنده نمی‌تواند بیش از 100 کاراکتر باشد.");

        RuleFor(x => x.AuthorEmail)
            .NotEmpty().WithMessage("ایمیل الزامی است.")
            .EmailAddress().WithMessage("ایمیل معتبر نیست.")
            .MaximumLength(200).WithMessage("ایمیل نمی‌تواند بیش از 200 کاراکتر باشد.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("محتوای کامنت الزامی است.")
            .MaximumLength(2000).WithMessage("کامنت نمی‌تواند بیش از 2000 کاراکتر باشد.");

        RuleFor(x => x.BlogPostId)
            .NotEmpty().WithMessage("شناسه مقاله الزامی است.");
    }
}

public class CreateBlogCategoryCommandValidator : AbstractValidator<Company.Application.Features.Blog.Commands.CreateBlogCategoryCommand>
{
    public CreateBlogCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryData)
            .NotNull().WithMessage("داده‌های دسته‌بندی الزامی است.")
            .SetValidator(new CreateOrUpdateBlogCategoryDtoValidator());
    }
}

public class CreateBlogPostCommandValidator : AbstractValidator<Company.Application.Features.Blog.Commands.CreateBlogPostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(x => x.PostData)
            .NotNull().WithMessage("داده‌های مقاله الزامی است.")
            .SetValidator(new CreateOrUpdateBlogPostDtoValidator());
    }
}

public class CreateBlogCommentCommandValidator : AbstractValidator<Company.Application.Features.Blog.Commands.CreateBlogCommentCommand>
{
    public CreateBlogCommentCommandValidator()
    {
        RuleFor(x => x.CommentData)
            .NotNull().WithMessage("داده‌های کامنت الزامی است.")
            .SetValidator(new CreateBlogCommentDtoValidator());
    }
}
