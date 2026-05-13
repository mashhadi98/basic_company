using Company.Application.Features.SiteSettings.DTOs;
using Company.Application.Features.SiteSettings.Queries;
using MediatR;

namespace Company.Application.Features.SiteSettings.Handlers;

public class GetSiteSettingsQueryHandler : IRequestHandler<GetSiteSettingsQuery, List<SiteSettingDto>>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public GetSiteSettingsQueryHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<SiteSettingDto>> Handle(GetSiteSettingsQuery request, CancellationToken cancellationToken)
    {
        var siteSettings = await _repository.GetAllAsync(request.IsPublished, request.Skip, request.Take, cancellationToken);
        return siteSettings.Select(MapToDto).ToList();
    }

    private static SiteSettingDto MapToDto(Domain.Entities.SiteSetting siteSetting)
    {
        return new SiteSettingDto
        {
            Id = siteSetting.Id,
            Key = siteSetting.Key,
            Value = siteSetting.Value,
            Type = siteSetting.Type,
            SortOrder = siteSetting.SortOrder,
            IsPublished = siteSetting.IsPublished,
            CreatedAt = siteSetting.CreatedAt,
            UpdatedAt = siteSetting.UpdatedAt,
            CreatedBy = siteSetting.CreatedBy,
            UpdatedBy = siteSetting.UpdatedBy
        };
    }
}

public class GetSiteSettingByIdQueryHandler : IRequestHandler<GetSiteSettingByIdQuery, SiteSettingDto?>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public GetSiteSettingByIdQueryHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SiteSettingDto?> Handle(GetSiteSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var siteSetting = await _repository.GetByIdAsync(request.SiteSettingId, cancellationToken);
        if (siteSetting == null)
            return null;

        return MapToDto(siteSetting);
    }

    private static SiteSettingDto MapToDto(Domain.Entities.SiteSetting siteSetting)
    {
        return new SiteSettingDto
        {
            Id = siteSetting.Id,
            Key = siteSetting.Key,
            Value = siteSetting.Value,
            Type = siteSetting.Type,
            SortOrder = siteSetting.SortOrder,
            IsPublished = siteSetting.IsPublished,
            CreatedAt = siteSetting.CreatedAt,
            UpdatedAt = siteSetting.UpdatedAt,
            CreatedBy = siteSetting.CreatedBy,
            UpdatedBy = siteSetting.UpdatedBy
        };
    }
}

public class GetSiteSettingByKeyQueryHandler : IRequestHandler<GetSiteSettingByKeyQuery, SiteSettingDto?>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public GetSiteSettingByKeyQueryHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SiteSettingDto?> Handle(GetSiteSettingByKeyQuery request, CancellationToken cancellationToken)
    {
        var siteSetting = await _repository.GetByKeyAsync(request.Key, cancellationToken);
        if (siteSetting == null)
            return null;

        return MapToDto(siteSetting);
    }

    private static SiteSettingDto MapToDto(Domain.Entities.SiteSetting siteSetting)
    {
        return new SiteSettingDto
        {
            Id = siteSetting.Id,
            Key = siteSetting.Key,
            Value = siteSetting.Value,
            Type = siteSetting.Type,
            SortOrder = siteSetting.SortOrder,
            IsPublished = siteSetting.IsPublished,
            CreatedAt = siteSetting.CreatedAt,
            UpdatedAt = siteSetting.UpdatedAt,
            CreatedBy = siteSetting.CreatedBy,
            UpdatedBy = siteSetting.UpdatedBy
        };
    }
}
