using Company.Application.Features.StaticPages.Commands;
using Company.Application.Features.StaticPages.Repositories;
using MediatR;

namespace Company.Application.Features.StaticPages.Handlers;

public class ToggleStaticPagePublishCommandHandler : IRequestHandler<ToggleStaticPagePublishCommand, bool>
{
    private readonly IStaticPageRepository _repository;

    public ToggleStaticPagePublishCommandHandler(IStaticPageRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(ToggleStaticPagePublishCommand request, CancellationToken cancellationToken)
    {
        var page = await _repository.GetByIdAsync(request.PageId, cancellationToken);
        if (page == null)
            return false;

        page.IsPublished = !page.IsPublished;
        await _repository.UpdateAsync(page, cancellationToken);
        return true;
    }
}