using Company.Application.Features.CompanyFeatures.Commands;
using Company.Application.Features.CompanyFeatures.DTOs;
using Company.Application.Features.CompanyFeatures.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Handlers;

public class CreateCompanyFeatureCommandHandler : IRequestHandler<CreateCompanyFeatureCommand, CompanyFeatureDto>
{
    private readonly ICompanyFeatureRepository _repository;

    public CreateCompanyFeatureCommandHandler(ICompanyFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<CompanyFeatureDto> Handle(CreateCompanyFeatureCommand request, CancellationToken cancellationToken)
    {
        var dto = request.FeatureData;

        var feature = new CompanyFeature
        {
            Title = dto.Title,
            Description = dto.Description,
            Icon = dto.Icon,
            SortOrder = dto.SortOrder,
            IsPublished = dto.IsPublished
        };

        await _repository.AddAsync(feature, cancellationToken);

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