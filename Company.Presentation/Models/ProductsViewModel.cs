using System;
using Company.Application.Features.Products.DTOs;

namespace Company.Presentation.Models;

public class ProductsIndexViewModel
{
    public List<ProductDto> Products { get; set; } = new();
    public List<ProductCategoryDto> Categories { get; set; } = new();
    public Guid? SelectedCategoryId { get; set; }
    public string? SelectedCategoryTitle { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 9;
    public int TotalCount { get; set; }
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 1;
}
