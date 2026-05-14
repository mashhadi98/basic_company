using Company.Application.Features.Blog.Commands;
using Company.Application.Features.Blog.DTOs;
using Company.Application.Features.Blog.Queries;
using Company.Presentation.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BlogPostsController : Controller
{
    private readonly IMediator _mediator;

    public BlogPostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(Guid? categoryId)
    {
        var query = new GetBlogPostsQuery { CategoryId = categoryId };
        var posts = await _mediator.Send(query);
        return View(posts);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _mediator.Send(new GetBlogCategoriesQuery { IsPublished = true });
        ViewBag.Categories = categories;
        return View(new BlogPostViewModel { PublishedDate = DateTime.UtcNow });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogPostViewModel model)
    {
        if (model.FeaturedImageFile != null && model.FeaturedImageFile.Length > 0)
        {
            var imagePath = await SaveBlogImageAsync(model.FeaturedImageFile);
            if (!string.IsNullOrEmpty(imagePath))
                model.FeaturedImage = imagePath;
        }

        if (!ModelState.IsValid)
        {
            var categories = await _mediator.Send(new GetBlogCategoriesQuery { IsPublished = true });
            ViewBag.Categories = categories;
            return View(model);
        }

        var command = new CreateBlogPostCommand
        {
            PostData = new CreateOrUpdateBlogPostDto
            {
                Title = model.Title,
                Slug = model.Slug,
                Content = model.Content,
                Summary = model.Summary,
                FeaturedImage = model.FeaturedImage,
                CategoryId = model.CategoryId,
                Author = model.Author,
                PublishedDate = model.PublishedDate,
                IsPublished = model.IsPublished,
                AllowComments = model.AllowComments,
                SortOrder = model.SortOrder,
                SeoMetaTitle = model.SeoMetaTitle,
                SeoMetaDescription = model.SeoMetaDescription,
                SeoKeywords = model.SeoKeywords,
                CanonicalUrl = model.CanonicalUrl
            }
        };

        await _mediator.Send(command);
        TempData["Success"] = "مقاله بلاگ با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var post = await _mediator.Send(new GetBlogPostByIdQuery { PostId = id });
        if (post == null)
            return NotFound();

        var model = new BlogPostViewModel
        {
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
            SortOrder = post.SortOrder,
            SeoMetaTitle = post.SeoMetaTitle,
            SeoMetaDescription = post.SeoMetaDescription,
            SeoKeywords = post.SeoKeywords,
            CanonicalUrl = post.CanonicalUrl
        };

        ViewBag.Categories = await _mediator.Send(new GetBlogCategoriesQuery { IsPublished = true });
        ViewBag.PostId = id;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, BlogPostViewModel model)
    {
        if (model.FeaturedImageFile != null && model.FeaturedImageFile.Length > 0)
        {
            var imagePath = await SaveBlogImageAsync(model.FeaturedImageFile);
            if (!string.IsNullOrEmpty(imagePath))
                model.FeaturedImage = imagePath;
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _mediator.Send(new GetBlogCategoriesQuery { IsPublished = true });
            ViewBag.PostId = id;
            return View(model);
        }

        var command = new UpdateBlogPostCommand
        {
            PostId = id,
            PostData = new CreateOrUpdateBlogPostDto
            {
                Title = model.Title,
                Slug = model.Slug,
                Content = model.Content,
                Summary = model.Summary,
                FeaturedImage = model.FeaturedImage,
                CategoryId = model.CategoryId,
                Author = model.Author,
                PublishedDate = model.PublishedDate,
                IsPublished = model.IsPublished,
                AllowComments = model.AllowComments,
                SortOrder = model.SortOrder,
                SeoMetaTitle = model.SeoMetaTitle,
                SeoMetaDescription = model.SeoMetaDescription,
                SeoKeywords = model.SeoKeywords,
                CanonicalUrl = model.CanonicalUrl
            }
        };

        await _mediator.Send(command);
        TempData["Success"] = "مقاله بلاگ با موفقیت ویرایش شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteBlogPostCommand { PostId = id });
        TempData[result ? "Success" : "Error"] = result ? "مقاله حذف شد." : "مقاله یافت نشد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        var result = await _mediator.Send(new ToggleBlogPostPublishCommand { PostId = id });
        TempData[result ? "Success" : "Error"] = result ? "وضعیت انتشار تغییر کرد." : "مقاله یافت نشد.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<string?> SaveBlogImageAsync(IFormFile featuredImageFile)
    {
        if (featuredImageFile == null || featuredImageFile.Length == 0)
            return null;

        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blog", "images");
        if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(featuredImageFile.FileName)}";
        var filePath = Path.Combine(uploadDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await featuredImageFile.CopyToAsync(stream);

        return $"/uploads/blog/images/{fileName}";
    }
}
