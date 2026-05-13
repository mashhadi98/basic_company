using Company.Application.Features.SiteSettings.DTOs;
using MediatR;

namespace Company.Application.Features.SiteSettings.Queries;

public class GetSiteSettingsQuery : IRequest<List<SiteSettingDto>>
{
    public bool? IsPublished { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
}

public class GetSiteSettingByIdQuery : IRequest<SiteSettingDto?>
{
    public Guid SiteSettingId { get; set; }
}

public class GetSiteSettingByKeyQuery : IRequest<SiteSettingDto?>
{
    public string Key { get; set; } = string.Empty;
}
