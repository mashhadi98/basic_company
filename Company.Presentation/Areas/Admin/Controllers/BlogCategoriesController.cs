using Company.Application.Features.Blog.Commands;
using Company.Application.Features.Blog.DTOs;
using Company.Application.Features.Blog.Queries;
using Company.Presentation.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class BlogCategoriesController : Controller
{
    private readonly IMediator _mediator;

    public BlogCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _mediator.Send(new GetBlogCategoriesQuery());
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new BlogCategoryViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogCategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var command = new CreateBlogCategoryCommand
        {
            CategoryData = new CreateOrUpdateBlogCategoryDto
            {
                Title = model.Title,
                Slug = model.Slug,
                Description = model.Description,
                SortOrder = model.SortOrder,
                IsPublished = model.IsPublished,
                SeoMetaTitle = model.SeoMetaTitle,
                SeoMetaDescription = model.SeoMetaDescription,
                SeoKeywords = model.SeoKeywords,
                CanonicalUrl = model.CanonicalUrl
            }
        };

        await _mediator.Send(command);
        TempData["Success"] = "دسته‌بندی بلاگ با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _mediator.Send(new GetBlogCategoryByIdQuery { CategoryId = id });
        if (category == null)
            return NotFound();

        var model = new BlogCategoryViewModel
        {
            Title = category.Title,
            Slug = category.Slug,
            Description = category.Description,
            SortOrder = category.SortOrder,
            IsPublished = category.IsPublished,
            SeoMetaTitle = category.SeoMetaTitle,
            SeoMetaDescription = category.SeoMetaDescription,
            SeoKeywords = category.SeoKeywords,
            CanonicalUrl = category.CanonicalUrl
        };

        ViewBag.CategoryId = id;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, BlogCategoryViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var command = new UpdateBlogCategoryCommand
        {
            CategoryId = id,
            CategoryData = new CreateOrUpdateBlogCategoryDto
            {
                Title = model.Title,
                Slug = model.Slug,
                Description = model.Description,
                SortOrder = model.SortOrder,
                IsPublished = model.IsPublished,
                SeoMetaTitle = model.SeoMetaTitle,
                SeoMetaDescription = model.SeoMetaDescription,
                SeoKeywords = model.SeoKeywords,
                CanonicalUrl = model.CanonicalUrl
            }
        };

        await _mediator.Send(command);
        TempData["Success"] = "دسته‌بندی بلاگ با موفقیت ویرایش شد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteBlogCategoryCommand { CategoryId = id });
        TempData[result ? "Success" : "Error"] = result ? "دسته‌بندی بلاگ حذف شد." : "دسته‌بندی یافت نشد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        var result = await _mediator.Send(new ToggleBlogCategoryPublishCommand { CategoryId = id });
        TempData[result ? "Success" : "Error"] = result ? "وضعیت انتشار تغییر کرد." : "دسته‌بندی یافت نشد.";
        return RedirectToAction(nameof(Index));
    }
}
