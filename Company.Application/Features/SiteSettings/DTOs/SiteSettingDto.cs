using Company.Domain.Entities;

namespace Company.Application.Features.SiteSettings.DTOs;

public class SiteSettingDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public SiteSettingType Type { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}

public class CreateOrUpdateSiteSettingDto
{
    public Guid? Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public SiteSettingType Type { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
}
