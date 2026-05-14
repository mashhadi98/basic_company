namespace Company.Application.Features.Customers.DTOs;

public class CustomerDto
{
    public Guid Id { get; set; }
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

public class CreateOrUpdateCustomerDto
{
    public Guid? Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? CompanyImage { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
}
