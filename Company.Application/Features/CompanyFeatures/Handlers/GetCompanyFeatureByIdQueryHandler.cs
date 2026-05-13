using Company.Application.Features.CompanyFeatures.DTOs;
using Company.Application.Features.CompanyFeatures.Queries;
using Company.Application.Features.CompanyFeatures.Repositories;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Handlers;

public class GetCompanyFeatureByIdQueryHandler : IRequestHandler<GetCompanyFeatureByIdQuery, CompanyFeatureDto?>
{
    private readonly ICompanyFeatureRepository _repository;

    public GetCompanyFeatureByIdQueryHandler(ICompanyFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<CompanyFeatureDto?> Handle(GetCompanyFeatureByIdQuery request, CancellationToken cancellationToken)
    {
        var feature = await _repository.GetByIdAsync(request.FeatureId, cancellationToken);
        if (feature == null)
            return null;

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