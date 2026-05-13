using Company.Application.Features.CompanyFeatures.Commands;
using Company.Application.Features.CompanyFeatures.Repositories;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Handlers;

public class ToggleCompanyFeaturePublishCommandHandler : IRequestHandler<ToggleCompanyFeaturePublishCommand, bool>
{
    private readonly ICompanyFeatureRepository _repository;

    public ToggleCompanyFeaturePublishCommandHandler(ICompanyFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleCompanyFeaturePublishCommand request, CancellationToken cancellationToken)
    {
        var feature = await _repository.GetByIdAsync(request.FeatureId, cancellationToken);
        if (feature == null)
            return false;

        feature.IsPublished = !feature.IsPublished;
        await _repository.UpdateAsync(feature, cancellationToken);
        return true;
    }
}