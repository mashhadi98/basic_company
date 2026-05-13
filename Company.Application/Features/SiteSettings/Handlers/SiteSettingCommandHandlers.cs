using Company.Application.Features.SiteSettings.Commands;
using Company.Application.Features.SiteSettings.DTOs;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.SiteSettings.Handlers;

public class CreateSiteSettingCommandHandler : IRequestHandler<CreateSiteSettingCommand, SiteSettingDto>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public CreateSiteSettingCommandHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SiteSettingDto> Handle(CreateSiteSettingCommand request, CancellationToken cancellationToken)
    {
        var dto = request.SiteSettingData;

        var siteSetting = new SiteSetting
        {
            Key = dto.Key,
            Value = dto.Value,
            Type = dto.Type,
            SortOrder = dto.SortOrder,
            IsPublished = dto.IsPublished,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(siteSetting, cancellationToken);

        return MapToDto(siteSetting);
    }

    private static SiteSettingDto MapToDto(SiteSetting siteSetting)
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

public class UpdateSiteSettingCommandHandler : IRequestHandler<UpdateSiteSettingCommand, SiteSettingDto>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public UpdateSiteSettingCommandHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<SiteSettingDto> Handle(UpdateSiteSettingCommand request, CancellationToken cancellationToken)
    {
        var dto = request.SiteSettingData;

        var siteSetting = await _repository.GetByIdAsync(request.SiteSettingId, cancellationToken);
        if (siteSetting == null)
            throw new InvalidOperationException($"تنظیم سایت با شناسه {request.SiteSettingId} یافت نشد.");

        siteSetting.Key = dto.Key;
        siteSetting.Value = dto.Value;
        siteSetting.Type = dto.Type;
        siteSetting.SortOrder = dto.SortOrder;
        siteSetting.IsPublished = dto.IsPublished;

        await _repository.UpdateAsync(siteSetting, cancellationToken);

        return MapToDto(siteSetting);
    }

    private static SiteSettingDto MapToDto(SiteSetting siteSetting)
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

public class DeleteSiteSettingCommandHandler : IRequestHandler<DeleteSiteSettingCommand, bool>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public DeleteSiteSettingCommandHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteSiteSettingCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.SiteSettingId, cancellationToken);
        if (!exists)
            return false;

        await _repository.DeleteAsync(request.SiteSettingId, cancellationToken);
        return true;
    }
}

public class ToggleSiteSettingPublishCommandHandler : IRequestHandler<ToggleSiteSettingPublishCommand, bool>
{
    private readonly Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository _repository;

    public ToggleSiteSettingPublishCommandHandler(Company.Application.Features.SiteSettings.Repositories.ISiteSettingRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleSiteSettingPublishCommand request, CancellationToken cancellationToken)
    {
        var siteSetting = await _repository.GetByIdAsync(request.SiteSettingId, cancellationToken);
        if (siteSetting == null)
            return false;

        siteSetting.IsPublished = !siteSetting.IsPublished;
        await _repository.UpdateAsync(siteSetting, cancellationToken);
        return true;
    }
}
