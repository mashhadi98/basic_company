using Company.Application.Features.SiteSettings.DTOs;
using MediatR;

namespace Company.Application.Features.SiteSettings.Commands;

public class CreateSiteSettingCommand : IRequest<SiteSettingDto>
{
    public CreateOrUpdateSiteSettingDto SiteSettingData { get; set; } = new();
}

public class UpdateSiteSettingCommand : IRequest<SiteSettingDto>
{
    public Guid SiteSettingId { get; set; }
    public CreateOrUpdateSiteSettingDto SiteSettingData { get; set; } = new();
}

public class DeleteSiteSettingCommand : IRequest<bool>
{
    public Guid SiteSettingId { get; set; }
}

public class ToggleSiteSettingPublishCommand : IRequest<bool>
{
    public Guid SiteSettingId { get; set; }
}
