using Company.Application.Features.CompanyFeatures.Commands;
using Company.Application.Features.CompanyFeatures.DTOs;
using Company.Application.Features.CompanyFeatures.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Handlers;

public class UpdateCompanyFeatureCommandHandler : IRequestHandler<UpdateCompanyFeatureCommand, CompanyFeatureDto>
{
    private readonly ICompanyFeatureRepository _repository;

    public UpdateCompanyFeatureCommandHandler(ICompanyFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<CompanyFeatureDto> Handle(UpdateCompanyFeatureCommand request, CancellationToken cancellationToken)
    {
        var dto = request.FeatureData;

        var feature = await _repository.GetByIdAsync(request.FeatureId, cancellationToken);
        if (feature == null)
            throw new InvalidOperationException($"ویژگی شرکت با شناسه {request.FeatureId} یافت نشد.");

        feature.Title = dto.Title;
        feature.Description = dto.Description;
        feature.Icon = dto.Icon;
        feature.SortOrder = dto.SortOrder;
        feature.IsPublished = dto.IsPublished;

        await _repository.UpdateAsync(feature, cancellationToken);

        return MapToDto(feature);
    }

    private CompanyFeatureDto MapToDto(CompanyFeature feature)
    {
        return new CompanyFeatureDto
        {
            Id = feature.Id,
            Title = feature.Title,
            Description = feature.Description,
            Icon = feature.Icon,
            SortOrder = feature.SortOrder,
            IsPublished = feature.IsPublished,
            CreatedAt = feature.CreatedAt,
            UpdatedAt = feature.UpdatedAt,
            CreatedBy = feature.CreatedBy,
            UpdatedBy = feature.UpdatedBy
        };
    }
}