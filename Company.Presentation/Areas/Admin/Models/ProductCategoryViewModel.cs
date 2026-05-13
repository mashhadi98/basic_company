using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Company.Presentation.Areas.Admin.Models;

public class ProductCategoryViewModel
{
    [Required(ErrorMessage = "عنوان دسته‌بندی لازم است.")]
    [MaxLength(256, ErrorMessage = "عنوان نمی‌تواند بیش از 256 کاراکتر باشد.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Slug لازم است.")]
    [MaxLength(256, ErrorMessage = "Slug نمی‌تواند بیش از 256 کاراکتر باشد.")]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "توضیحات نمی‌تواند بیش از 1000 کاراکتر باشد.")]
    public string? Description { get; set; }

    public Guid? ParentCategoryId { get; set; }

    [MaxLength(512, ErrorMessage = "آدرس تصویر نمی‌تواند بیش از 512 کاراکتر باشد.")]
    public string? Image { get; set; }

    public IFormFile? CategoryImageFile { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "ترتیب نمایش باید عددی غیرمنفی باشد.")]
    public int SortOrder { get; set; }

    public bool IsPublished { get; set; }

    [MaxLength(160, ErrorMessage = "عنوان سئو نمی‌تواند بیش از 160 کاراکتر باشد.")]
    public string? SeoMetaTitle { get; set; }

    [MaxLength(160, ErrorMessage = "توضیحات سئو نمی‌تواند بیش از 160 کاراکتر باشد.")]
    public string? SeoMetaDescription { get; set; }
}
