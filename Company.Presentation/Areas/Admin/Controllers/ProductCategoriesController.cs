using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductCategoriesController : Controller
{
    private readonly IMediator _mediator;

    public ProductCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// لیست دسته‌بندی محصولات
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _mediator.Send(new GetProductCategoriesQuery());
        return View(categories);
    }
}
