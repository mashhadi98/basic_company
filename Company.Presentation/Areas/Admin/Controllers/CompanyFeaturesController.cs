using Company.Application.Features.CompanyFeatures.Commands;
using Company.Application.Features.CompanyFeatures.DTOs;
using Company.Application.Features.CompanyFeatures.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CompanyFeaturesController : Controller
{
    private readonly IMediator _mediator;

    public CompanyFeaturesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: Admin/CompanyFeatures
    public async Task<IActionResult> Index(bool? isPublished)
    {
        var query = new GetCompanyFeaturesQuery { IsPublished = isPublished };
        var features = await _mediator.Send(query);
        return View(features);
    }

    // GET: Admin/CompanyFeatures/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/CompanyFeatures/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrUpdateCompanyFeatureDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var command = new CreateCompanyFeatureCommand { FeatureData = dto };
        await _mediator.Send(command);

        TempData["Success"] = "ویژگی شرکت با موفقیت ایجاد شد.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/CompanyFeatures/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var query = new GetCompanyFeatureByIdQuery { FeatureId = id };
        var feature = await _mediator.Send(query);

        if (feature == null)
            return NotFound();

        var dto = new CreateOrUpdateCompanyFeatureDto
        {
            Id = feature.Id,
            Title = feature.Title,
            Description = feature.Description,
            Icon = feature.Icon,
            SortOrder = feature.SortOrder,
            IsPublished = feature.IsPublished
        };

        return View(dto);
    }

    // POST: Admin/CompanyFeatures/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateOrUpdateCompanyFeatureDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var command = new UpdateCompanyFeatureCommand { FeatureId = id, FeatureData = dto };
        await _mediator.Send(command);

        TempData["Success"] = "ویژگی شرکت با موفقیت ویرایش شد.";
        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/CompanyFeatures/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCompanyFeatureCommand { FeatureId = id };
        var result = await _mediator.Send(command);

        if (result)
            TempData["Success"] = "ویژگی شرکت با موفقیت حذف شد.";
        else
            TempData["Error"] = "ویژگی شرکت یافت نشد.";

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/CompanyFeatures/TogglePublish/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        var command = new ToggleCompanyFeaturePublishCommand { FeatureId = id };
        var result = await _mediator.Send(command);

        if (result)
            TempData["Success"] = "وضعیت انتشار تغییر یافت.";
        else
            TempData["Error"] = "ویژگی شرکت یافت نشد.";

        return RedirectToAction(nameof(Index));
    }

    // POST: Admin/CompanyFeatures/Reorder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reorder(List<ReorderCompanyFeaturesCommand.ReorderItem> items)
    {
        var command = new ReorderCompanyFeaturesCommand { Items = items };
        await _mediator.Send(command);

        return Json(new { success = true });
    }
}