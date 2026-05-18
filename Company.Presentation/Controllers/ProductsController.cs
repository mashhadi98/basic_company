using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Queries;
using Company.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Controllers;

public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(Guid? categoryId = null, int page = 1, int pageSize = 9)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Max(pageSize, 1);
        var skip = (page - 1) * pageSize;

        var products = await _mediator.Send(new GetProductsQuery
        {
            CategoryId = categoryId,
            IsPublished = true,
            Skip = skip,
            Take = pageSize
        });

        var totalCount = await _mediator.Send(new GetProductsCountQuery
        {
            CategoryId = categoryId,
            IsPublished = true
        });

        var categories = await _mediator.Send(new GetProductCategoriesQuery
        {
            IsPublished = true
        });

        var viewModel = new ProductsIndexViewModel
        {
            Products = products,
            Categories = categories,
            SelectedCategoryId = categoryId,
            SelectedCategoryTitle = categoryId.HasValue
                ? categories.FirstOrDefault(c => c.Id == categoryId)?.Title
                : null,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        return View(viewModel);
    }

    [Route("/products/{id:guid}")]
    public async Task<IActionResult> Single(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery { ProductId = id });
        if (product == null)
            return NotFound();

        var categories = await _mediator.Send(new GetProductCategoriesQuery { IsPublished = true });
        var categoryTitle = categories.FirstOrDefault(c => c.Id == product.CategoryId)?.Title;
        ViewBag.CategoryTitle = categoryTitle;

        return View(product);
    }
}
