using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// Customer or partner company shown on the website.
/// </summary>
public class Customer : BaseEntity
{
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CompanyImage { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
