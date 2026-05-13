using Company.Application.Features.CompanyFeatures.DTOs;
using Company.Application.Features.CompanyFeatures.Queries;
using Company.Application.Features.CompanyFeatures.Repositories;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Handlers;

public class GetCompanyFeaturesQueryHandler : IRequestHandler<GetCompanyFeaturesQuery, List<CompanyFeatureDto>>
{
    private readonly ICompanyFeatureRepository _repository;

    public GetCompanyFeaturesQueryHandler(ICompanyFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CompanyFeatureDto>> Handle(GetCompanyFeaturesQuery request, CancellationToken cancellationToken)
    {
        var features = await _repository.GetAllAsync(request.IsPublished, request.Skip, request.Take, cancellationToken);

        return features.Select(MapToDto).ToList();
    }

    private CompanyFeatureDto MapToDto(Domain.Entities.CompanyFeature feature)
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