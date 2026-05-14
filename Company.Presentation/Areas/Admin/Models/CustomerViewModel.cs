using Microsoft.AspNetCore.Http;

namespace Company.Presentation.Areas.Admin.Models;

public class CustomerViewModel
{
    public Guid? Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CompanyImage { get; set; }
    public IFormFile? CompanyImageFile { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
}
