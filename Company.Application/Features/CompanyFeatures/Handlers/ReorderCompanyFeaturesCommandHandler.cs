using Company.Application.Features.CompanyFeatures.Commands;
using Company.Application.Features.CompanyFeatures.Repositories;
using MediatR;

namespace Company.Application.Features.CompanyFeatures.Handlers;

public class ReorderCompanyFeaturesCommandHandler : IRequestHandler<ReorderCompanyFeaturesCommand, bool>
{
    private readonly ICompanyFeatureRepository _repository;

    public ReorderCompanyFeaturesCommandHandler(ICompanyFeatureRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ReorderCompanyFeaturesCommand request, CancellationToken cancellationToken)
    {
        foreach (var item in request.Items)
        {
            var feature = await _repository.GetByIdAsync(item.Id, cancellationToken);
            if (feature != null)
            {
                feature.SortOrder = item.SortOrder;
                await _repository.UpdateAsync(feature, cancellationToken);
            }
        }

        return true;
    }
}